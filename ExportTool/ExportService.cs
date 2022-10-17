using CsvHelper;
using Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExportTool
{
    public class ExportService
    {
        public async Task ExportClientListToCsvAsync(IEnumerable<Client> clientList, string pathToDirectory, string csvFileName)
        {
            var dirInfo = new DirectoryInfo(pathToDirectory);

            if (!dirInfo.Exists)
                dirInfo.Create();

            var fullPath = Path.Combine(pathToDirectory, csvFileName);

            await using var fileStream = new FileStream(fullPath, FileMode.Create);
            await using var streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
            await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.CurrentCulture);

            await csvWriter.WriteRecordsAsync(clientList);
            await csvWriter.FlushAsync();
        }

        public async Task<List<Client>> ReadClientListFromCsvAsync(string pathToDirectory, string csvFileName)
        {
            var fullPath = Path.Combine(pathToDirectory, csvFileName);

            await using var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
            using var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8);
            using var csvReader = new CsvReader(streamReader, CultureInfo.CurrentCulture);

            return csvReader.GetRecords<Client>().ToList();
        }
    }
}