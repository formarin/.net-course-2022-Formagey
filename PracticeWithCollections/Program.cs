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
            int count = 10;

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


            for (int i = 0; i < count; i++)
            {
                sw.Start();
                var foundClients1 = clientList.FirstOrDefault(client => client.PhoneNumber == lastListClientPhone);
                sw.Stop();
            }
            Console.WriteLine($"{sw.ElapsedTicks / count,5} in list comparing by client.PhoneNumber");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                sw.Start();
                var foundClients11 = clientList.FirstOrDefault(client => client == lastListClient);
                sw.Stop();
            }
            Console.WriteLine($"{sw.ElapsedTicks / count,5} in list comparing by client");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                sw.Start();
                var foundClients2 = clientDictionary.FirstOrDefault(client => client.Value.PhoneNumber == lastDictionaryClientPhone);
                sw.Stop();
            }
            Console.WriteLine($"{sw.ElapsedTicks / count,5} in dictionary comparing by client.PhoneNumber");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                sw.Start();
                var foundClients22 = clientDictionary.FirstOrDefault(client => client.Value == lastDictionaryClient);
                sw.Stop();
            }
            Console.WriteLine($"{sw.ElapsedTicks / count,5} in dictionary comparing by client");

            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                sw.Start();
                var foundClients3 = clientDictionary[lastDictionaryClientPhone];
                sw.Stop();
            }
            Console.WriteLine($"{sw.ElapsedTicks / count,5} in dictionary by index");


            var clientsAgedLess30 = clientList.Where(client => (DateTime.Now.Year - client.DateOfBirth.Year) < 30).ToList();

            var minSalary = employeeList.Min(employee => employee.Salary);
            var foundEmployee = employeeList.Where(employee => employee.Salary == minSalary).ToList();

            Console.ReadKey();
        }
    }
}