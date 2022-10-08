using ExportTool;
using Services;
using System.IO;
using Xunit;

namespace ServiceTests
{
    public class ExportToolTests
    {
        [Fact]
        public void ExportData_PositiveTest()
        {
            //Arrange
            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "..", "..", "..", "..", "ExportTool", "ExportData");
            var exportService = new ExportService();
            var clientList = new TestDataGenerator().GetClientList(5);

            //Act
            exportService.ExportClientListToCsv(clientList, path, "Clients.csv");
            var listFromCSV = exportService.ReadClientListFromCsv(path, "Clients.csv");

            //Assert
            Assert.Equal(listFromCSV, clientList);
        }
    }
}
