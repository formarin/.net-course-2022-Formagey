using ExportTool;
using Models;
using Services;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class ExportToolTests
    {
        [Fact]
        public async Task ExportDataToCsv_PositiveTestAsync()
        {
            //Arrange
            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "..", "..", "..", "ExportTool", "ExportData");
            var exportService = new ExportService();
            var clientList = new TestDataGenerator().GetClientList(5);

            //Act
            await exportService.ExportClientListToCsvAsync(clientList, path, "Clients.csv");
            var listFromCSV = await exportService.ReadClientListFromCsvAsync(path, "Clients.csv");

            //Assert
            Assert.Equal(listFromCSV, clientList);
        }

        [Fact]
        public async Task ExportClientsToTxt_PositiveTestAsync()
        {
            //Arrange
            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "..", "..", "..", "ExportTool", "ExportData");
            var exportService = new ExportService();
            var clientList = new TestDataGenerator().GetClientList(5);

            //Act
            await exportService.ExportToTxtAsync(clientList, path, "Clients.txt");
            var listFromTxt = await exportService.ImportFromTxtAsync<Client>(path, "Clients.txt");

            //Assert
            Assert.Equal(listFromTxt, clientList);
        }

        [Fact]
        public async Task ExportEmployeeToTxt_PositiveTestAsync()
        {
            //Arrange
            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "..", "..", "..", "ExportTool", "ExportData");
            var exportService = new ExportService();
            var employeeList = new TestDataGenerator().GetEmployeeList(5);

            //Act
            await exportService.ExportToTxtAsync(employeeList, path, "Employees.txt");
            var listFromTxt = await exportService.ImportFromTxtAsync<Employee>(path, "Employees.txt");

            //Assert
            Assert.Equal(listFromTxt, employeeList);
        }
    }
}
