using CsvHelper;
using Models;
using Newtonsoft.Json;
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

        public async Task ExportToTxtAsync<T>(IEnumerable<T> creatureColection, string pathToDirectory, string csvFileName)
        {
            var dirInfo = new DirectoryInfo(pathToDirectory);

            if (!dirInfo.Exists)
                dirInfo.Create();

            var fullPath = Path.Combine(pathToDirectory, csvFileName);

            await using var fileStream = new FileStream(fullPath, FileMode.Create);
            await using var streamWriter = new StreamWriter(fileStream, System.Text.Encoding.UTF8);

            streamWriter.Write(JsonConvert.SerializeObject(creatureColection));
        }

        public async Task<IEnumerable<T>> ImportFromTxtAsync<T>(string pathToDirectory, string csvFileName)
        {
            var fullPath = Path.Combine(pathToDirectory, csvFileName);

            await using var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
            using var streamReader = new StreamReader(fileStream, System.Text.Encoding.UTF8);
            
            return JsonConvert.DeserializeObject<IEnumerable<T>>(await streamReader.ReadToEndAsync());
        }
    }
}