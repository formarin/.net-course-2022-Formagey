using Models;
using Services.Exceptions;
using System;
using System.Collections.Generic;

namespace Services
{
    public class EmployeeService
    {
        private List<Employee> _employeeList = new();
        public void AddEmployee(Employee employee)
        {
            if (DateTime.Now.Year - employee.DateOfBirth.Year < 18)
            {
                throw new AgeLimitException("Минимально допустимый возраст: 18 лет.");
            }
            if (employee.FirstName == null || employee.LastName == null || employee.DateOfBirth == new DateTime(0))
            {
                throw new NoPassportDataException("Наличие всех паспортных данных обязательно.");
            }

            _employeeList.Add(employee);
        }
    }
}
