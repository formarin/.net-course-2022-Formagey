using Models;

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