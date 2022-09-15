using Models;
using System.Collections.Generic;

namespace Services
{
    public class BankService
    {
        public List<Person> BlackList = new List<Person>();

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

        public void AddBonus<T>(T person) where T : Person
        {
            person.BonusCount++;
        }

        public void AddToBlackList<T>(T person) where T : Person
        {
            BlackList.Add(person);
        }

        public bool IsPersonInBlackList<T>(T person) where T : Person
        {
            return BlackList.Contains(person);
        }
    }
}
