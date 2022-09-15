using Models;
using Services.Storages;
using System.Collections.Generic;
using System.Linq;

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

        public void Update(Employee employee)
        {
            var existingEmployee = Data.FirstOrDefault(x => x == employee);
            existingEmployee = employee;
        }

        public void Delete(Employee employee)
        {
            Data.Remove(employee);
        }
    }
}
