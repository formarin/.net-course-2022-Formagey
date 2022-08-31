using Models;
using Services;

namespace PracticeWithTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            var employee = new Employee
            {
                FirstName = "firstName",
                LastName = "lastName"
            };

            var usd = new Currency
            {
                Code = 840,
                Name = "usd"
            };

            BadUpdateEmployeeContract(employee);
            employee.Contract = null;
            employee.Contract = UpdateEmployeeContract(employee.FirstName, employee.LastName);

            BadUpdateCurrency(usd);
            UpdateCurrency(ref usd);


            var bankService = new BankService();
            employee.Salary = bankService.GetOwnerSalary(123456.1, 1234, 4);

            var client = new Client
            {
                FirstName = "firstName2",
                LastName = "lastNam2"
            };
            employee = bankService.GetEmployeeFromClient(client);
        }

        public static void BadUpdateEmployeeContract(Employee employee)
        {
            employee.Contract = $"{employee.FirstName} {employee.LastName}";
        }

        public static string UpdateEmployeeContract(string firstName, string lastName)
        {
            return $"{firstName} {lastName}";
        }

        public static void BadUpdateCurrency(Currency currency)
        {
            currency.Name = "rup";
        }

        public static void UpdateCurrency(ref Currency currency)
        {
            currency.Name = "rup";
        }
    }
}