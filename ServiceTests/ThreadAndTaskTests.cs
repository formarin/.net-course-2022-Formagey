using ExportTool;
using Models;
using Services;
using Services.Filters;
using System.IO;
using System.Threading;
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
        public void Test()
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
        public void Test2()
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
    }
}
