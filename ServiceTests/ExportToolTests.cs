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
            var exportService = new ExportService(path, "Clients.csv");

            var testDataGenerator = new TestDataGenerator();
            var list = testDataGenerator.GetClientList(5);

            //Act
            exportService.ExportClientListToCsv(list);

            var listFromCSV = exportService.ReadClientFromCsv();

            //Assert
            Assert.Equal(listFromCSV, list);
        }
    }
}
