using Models;
using Services;
using Services.Exceptions;
using System;
using Xunit;

namespace ServiceTests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void AddEmployee_throwsAgeLimitException()
        {
            //Arrange
            var employeeService = new EmployeeService();
            var employee = new Employee
            {
                DateOfBirth = new DateTime(2020, 1, 1)
            };

            //Act Assert
            Assert.Throws<AgeLimitException>(() => employeeService.AddEmployee(employee));
        }

        [Fact]
        public void AddEmployee_throwsNoPassportDataException()
        {
            //Arrange
            var employeeService = new EmployeeService();
            var employee = new Employee
            {
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act Assert
            Assert.Throws<NoPassportDataException>(() => employeeService.AddEmployee(employee));
        }
    }
}
