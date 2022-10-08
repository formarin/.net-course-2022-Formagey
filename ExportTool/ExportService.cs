using CsvHelper;
using Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ExportTool
{
    public class ExportService
    {
        public void ExportClientListToCsv(IEnumerable<Client> clientList, string pathToDirectory, string csvFileName)
        {
            var dirInfo = new DirectoryInfo(pathToDirectory);

            if (!dirInfo.Exists)
                dirInfo.Create();

            var fullPath = Path.Combine(pathToDirectory, csvFileName);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            using var streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
            using var csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture);

            csvWriter.WriteRecords(clientList);
            csvWriter.Flush();
        }

        public List<Client> ReadClientListFromCsv(string pathToDirectory, string csvFileName)
        {
            var fullPath = Path.Combine(pathToDirectory, csvFileName);

            using var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
            using var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8);
            using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

            return csvReader.GetRecords<Client>().ToList();
        }
    }
}