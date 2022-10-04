using CsvHelper;
using Models;
using Services;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ExportTool
{
    public class ExportService
    {
        private string _pathToDirectory { get; set; }
        private string _csvFileName { get; set; }

        public ExportService(string pathToDirectory, string csvFileName)
        {
            _pathToDirectory = pathToDirectory;
            _csvFileName = csvFileName;
        }

        public void ExportClientListToCsv(IEnumerable<Client> clientList)
        {
            var dirInfo = new DirectoryInfo(_pathToDirectory);

            if (!dirInfo.Exists)
                dirInfo.Create();

            var fullPath = Path.Combine(_pathToDirectory, _csvFileName);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture);

            csvWriter.WriteRecords(clientList);
            csvWriter.Flush();
        }

        public List<Client> ReadClientFromCsv()
        {
            var fullPath = Path.Combine(_pathToDirectory, _csvFileName);

            using var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
            using var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8);
            using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

            return csvReader.GetRecords<Client>().ToList();
        }

        public void ImportClientsToDatabaseFromCsv()
        {
            var list = ReadClientFromCsv();

            var clientService = new ClientService();
            clientService.AddClientList(list);
        }
    }
}