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
            var clientService = new ClientService();
            var locker = new object();

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "..", "..", "..", "ExportTool", "ExportData");

            void ImportClients()
            {
                lock (locker)
                {
                    var clientList = exportService.ReadClientListFromCsv(path, "Clients.csv");
                    clientService.AddClientList(clientList);
                }
            };

            void ExportClients()
            {
                lock (locker)
                {
                    var clientList = clientService.GetClients(new ClientFilter { pageNumber = 1, notesCount = 5 });
                    exportService.ExportClientListToCsv(clientList, path, "ClientsToExport.csv");
                }
            };

            var exportClients = new Thread(ExportClients) { Name = "exporting thread" };
            exportClients.Start();

            var importClients = new Thread(ImportClients) { Name = "importing thread" };
            importClients.Start();

            Thread.Sleep(10000);
        }

        [Fact]
        public void BackgroundRateUpdater_PositiveTest()
        {
            new ClientService().AddClientList(new TestDataGenerator().GetClientList(10));

            var cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;

            var rateUpdaterService = new RateUpdaterService();
            var task = rateUpdaterService.RateUpdater(token);

            task.Start();
            task.Wait(30000);

            cancelTokenSource.Cancel();
        }

        [Fact]
        public void ParallelCashOUt_PositiveTest()
        {
            var clientService = new ClientService();
            var clientList = new TestDataGenerator().GetClientList(1000);
            var accountList = new List<Account>();

            clientService.AddClientList(clientList);
            for (var i = 0; i < clientList.Count; i++)
            {
                var account = clientService.GetClient(clientList[i].Id)
                    .AccountCollection.FirstOrDefault(x => x.CurrencyName == "USD");
                account.Amount += 1000;
                clientService.UpdateAccount(account);
                accountList.Add(account);
            }

            var cashDispenserService = new CashDispenserService();
            var tasks = new Task[accountList.Count];

            for (var i = 0; i < tasks.Length; i++)
                tasks[i] = cashDispenserService.CashOut(100, accountList[i]);

            foreach (var t in tasks)
                t.Start();

            Task.WaitAll(tasks);
        }
    }
}
