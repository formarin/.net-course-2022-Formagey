using Models;
using Services.Storages;
using System.Collections.Generic;

namespace Services
{
    public class EmployeeStorage : IEmployeeStorage
    {
        public List<Employee> Data { get; }

        public EmployeeStorage()
        {
            Data = new List<Employee>();
        }

        public void Add(Employee employee)
        {
            Data.Add(employee);
        }

        public void Add(List<Employee> employeeList)
        {
            foreach (var employee in employeeList)
            {
                Data.Add(employee);
            }
        }

        public void Update(Employee employee)
        {
            Data.Remove(employee);
            Data.Add(employee);
        }

        public void Delete(Employee employee)
        {
            Data.Remove(employee);
        }
    }
}
