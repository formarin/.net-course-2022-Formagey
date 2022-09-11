using Models;
using Services.Exceptions;
using Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class EmployeeService
    {
        private EmployeeStorage _employeeStorage = new();

        public EmployeeService(EmployeeStorage employeeStorage)
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

        public List<Employee> GetEmployees(EmployeeFilter filter)
        {
            var query = _employeeStorage._employeeList.Where(_ => true);

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
