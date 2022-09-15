using Models;
using Services.Exceptions;
using Services.Filters;
using Services.Storages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class EmployeeService<T> where T : IEmployeeStorage
    {
        public T _employeeStorage;

        public EmployeeService(T employeeStorage)
        {
            _employeeStorage = employeeStorage;
        }

        public void AddEmployee(Employee employee)
        {
            if (DateTime.Now.Year - employee.DateOfBirth.Year < 18)
            {
                throw new AgeLimitException("Минимально допустимый возраст: 18 лет.");
            }
            if (employee.FirstName == null | employee.LastName == null | employee.PassportNumber == 0 | employee.DateOfBirth == new DateTime(0))
            {
                throw new NoPassportDataException("Наличие всех паспортных данных обязательно.");
            }

            _employeeStorage.Add(employee);
        }

        public void AddEmployeeList(List<Employee> employeeList)
        {
            foreach (var employee in employeeList)
            {
                AddEmployee(employee);
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            if (_employeeStorage.Data.Contains(employee))
                _employeeStorage.Update(employee);
            else
                throw new ArgumentException("В базе данных нет такого сотрудника");
        }

        public void DeleteEmployee(Employee employee)
        {
            _employeeStorage.Delete(employee);
        }

        public List<Employee> GetEmployees(EmployeeFilter filter)
        {
            var query = _employeeStorage.Data.Where(_ => true);

            if (filter.FirstName != null)
                query = query.Where(x => x.FirstName == filter.FirstName);

            if (filter.LastName != null)
                query = query.Where(x => x.LastName == filter.LastName);

            if (filter.PhoneNumber != null)
                query = query.Where(x => x.PhoneNumber == filter.PhoneNumber);

            if (filter.PassportNumber != null)
                query = query.Where(x => x.PassportNumber == filter.PassportNumber);

            if (filter.MinDate != null)
                query = query.Where(x => x.DateOfBirth >= filter.MinDate);

            if (filter.MaxDate != null)
                query = query.Where(x => x.DateOfBirth <= filter.MaxDate);

            return query.ToList();
        }
    }
}
