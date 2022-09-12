using Models;
using System.Collections.Generic;

namespace Services
{
    public class EmployeeStorage : IStorage
    {
        public readonly List<Employee> _employeeList = new();
        public void Add(Person person)
        {
            var employee = person as Employee;
            _employeeList.Add(employee);
        }
        public void Add(List<Employee> employeeList)
        {
            foreach (var employee in employeeList)
            {
                _employeeList.Add(employee);
            }
        }
        public void Update(Person person)
        {

        }
        public void Delete(Person person)
        {

        }
    }
}
