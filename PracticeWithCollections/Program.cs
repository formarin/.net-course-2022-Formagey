using Models;
using Services;
using System;
using System.Diagnostics;
using System.Linq;

namespace PracticeWithCollections
{
    class Program
    {
        static void Main(string[] args)
        {
            var testDataGenerator = new TestDataGenerator();
            var employeeList = testDataGenerator.GetEmployeeList();
            var clientList = testDataGenerator.GetClientList();
            var clientDictionary = testDataGenerator.GetClientDictionary();
            var sw = new Stopwatch();

            var lastListClientPhone = clientList.Last().PhoneNumber;
            var lastListClient = new Client
            {
                PhoneNumber = lastListClientPhone
            };

            var lastDictionaryClientPhone = clientDictionary.Last().Key;
            var lastDictionaryClient = new Client
            {
                PhoneNumber = lastDictionaryClientPhone
            };

            sw.Start();
            var foundClients1 = clientList.FirstOrDefault(client => client.PhoneNumber == lastListClientPhone);
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);

            sw.Restart();
            var foundClients11 = clientList.FirstOrDefault(client => client == lastListClient);
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);

            sw.Restart();
            var foundClients2 = clientDictionary.FirstOrDefault(client => client.Value.PhoneNumber == lastDictionaryClientPhone);
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);

            sw.Restart();
            var foundClients22 = clientDictionary.FirstOrDefault(client => client.Value == lastDictionaryClient);
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);

            sw.Restart();
            var foundClients3 = clientDictionary[lastDictionaryClientPhone];
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);

            Console.ReadKey();
        }
    }
}