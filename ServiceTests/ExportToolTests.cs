using ExportTool;
using Services;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class ExportToolTests
    {
        [Fact]
        public async Task ExportData_PositiveTestAsync()
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
    }
}
