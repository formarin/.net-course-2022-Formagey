using ExportTool;
using Models;
using Services;
using Services.Filters;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ServiceTests
{
    public class ThreadAndTaskTests
    {
        private ITestOutputHelper _outPut;

        public ThreadAndTaskTests(ITestOutputHelper outPut)
        {
            _outPut = outPut;
        }

        [Fact]
        public void ParallelMoneyEdition_PositiveTest()
        {
            var locker = new object();
            var account = new Account { Amount = 0 };

            void AddMoney()
            {
                for (var i = 0; i < 10; i++)
                {
                    lock (locker)
                    {
                        account.Amount += 100;
                        _outPut.WriteLine($"{Thread.CurrentThread.Name} добавил 100$, " +
                                          $"текущая сумма: {account.Amount}");
                    }
                    Thread.Sleep(10);
                }
            }

            var threadA = new Thread(AddMoney) { Name = "Thread A" };
            threadA.Start();

            var threadB = new Thread(AddMoney) { Name = "Thread B" };
            threadB.Start();

            Thread.Sleep(10000);
        }

        [Fact]
        public void ParallelImportAndExportData_PositiveTest()
        {
            var exportService = new ExportService();
            var clientService1 = new ClientService();
            var clientService2 = new ClientService();
            var locker = new object();

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "..", "..", "..", "ExportTool", "ExportData");

            async Task ImportClientsAsync()
            {
                var clientList = await exportService.ReadClientListFromCsvAsync(path, "Clients.csv");
                await clientService1.AddClientListAsync(clientList);
            };

            async Task ExportClientsAsync()
            {
                var clientList = await clientService2.GetClientsAsync(new ClientFilter
                {
                    pageNumber = 1,
                    notesCount = 5
                });
                await exportService.ExportClientListToCsvAsync(clientList, path, "ClientsToExport.csv");
            };

            var exportClients = new Thread(async () => await ExportClientsAsync()) { Name = "exporting thread" };
            exportClients.Start();

            var importClients = new Thread(async () => await ImportClientsAsync()) { Name = "importing thread" };
            importClients.Start();

            Thread.Sleep(10000);
        }

        [Fact]
        public async Task BackgroundRateUpdater_PositiveTestAsync()
        {
            await new ClientService().AddClientListAsync(new TestDataGenerator().GetClientList(10));

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;

            var rateUpdaterService = new RateUpdaterService();
            var task = rateUpdaterService.RateUpdater(token);

            task.Wait(5000);

            cancelTokenSource.Cancel();
        }

        [Fact]
        public async Task ParallelCashOUt_PositiveTestAsync()
        {
            var clientService = new ClientService();
            var clientList = new TestDataGenerator().GetClientList(1000);
            var accountList = new List<Account>();

            await clientService.AddClientListAsync(clientList);
            for (var i = 0; i < clientList.Count; i++)
            {
                var client = await clientService.GetClientAsync(clientList[i].Id);
                var account = client.AccountCollection.FirstOrDefault(x => x.CurrencyName == "USD");
                account.Amount += 1000;
                await clientService.UpdateAccountAsync(account);
                accountList.Add(account);
            }

            var cashDispenserService = new CashDispenserService();
            var tasks = new Task[accountList.Count];

            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = cashDispenserService.CashOut(100, accountList[i]);

            Task.WaitAll(tasks);
        }

        [Fact]
        public async Task Threadpool_TestAsync()
        {
            ThreadPool.SetMaxThreads(10, 10);
            ThreadPool.GetAvailableThreads(out var workerThreads, out var completionPortThreads);

            _outPut.WriteLine(workerThreads.ToString());
            _outPut.WriteLine(completionPortThreads.ToString());

            var clientService = new ClientService();
            var tasks = new Task[500];

            for (var i = 0; i < workerThreads; i++)
            {
                var task = clientService.GetClientsAsync(new ClientFilter
                {
                    pageNumber = 1,
                    notesCount = 10
                });
                _outPut.WriteLine($"   {i} я задача началась");
                tasks[i] = task;
                ThreadPool.GetAvailableThreads(out var workerThreadsN, out var completionPortThreadsN);
                _outPut.WriteLine(workerThreadsN.ToString());
            }

            await Task.Run(async () =>
            {
                var сlientService2 = new ClientService();
                var clientList = new TestDataGenerator().GetClientList(1);
                await сlientService2.AddClientAsync(clientList[0]);
            });

            Task.WaitAll();
        }
    }
}
