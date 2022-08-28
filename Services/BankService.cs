using Models;
using System;

namespace Services
{
    public class BankService
    {
        public int GetOwnerSalary(double income, double expenses, int ownersCount)
        {
            return (int)((income - expenses) / ownersCount);
        }

        public Employee GetEmployeeFromClient(Client client)
        {
            return new Employee
            {
                FirstName = client.FirstName,
                LastName = client.LastName
            };
        }
    }
}
