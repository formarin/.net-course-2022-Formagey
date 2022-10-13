using ExportTool;
using Models;
using Services;
using Services.Filters;
using System;
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
            var clientServiceForImport = new ClientService();
            var clientServiceForExport = new ClientService();

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "..", "..", "..", "ExportTool", "ExportData");

            void ImportClients()
            {
                for (var i = 0; i < 10; i++)
                {
                    var clientList = exportService.ReadClientListFromCsv(path, "Clients.csv");
                    
                    for (var j = 0; j < clientList.Count; j++)
                        clientList[j].Id = Guid.NewGuid();
                    
                    clientServiceForImport.AddClientList(clientList);

                    Thread.Sleep(100);
                }
            };

            void ExportClients()
            {
                for (var i = 0; i < 10; i++)
                {
                    var clientList = clientServiceForExport.GetClients(new ClientFilter { pageNumber = 1, notesCount = 5 });
                    exportService.ExportClientListToCsv(clientList, path, "ClientsToExport.csv");
                    
                    Thread.Sleep(100);
                }
            };

            var exportClients = new Thread(ExportClients) { Name = "exporting thread" };
            var importClients = new Thread(ImportClients) { Name = "importing thread" };

            exportClients.Start();
            importClients.Start();

            Thread.Sleep(10000);
        }
    }
}
