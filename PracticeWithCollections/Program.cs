using Services;

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
        }
    }
}
