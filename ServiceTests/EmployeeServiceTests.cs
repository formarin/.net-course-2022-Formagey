﻿using Models;
using Services;
using Services.Exceptions;
using Services.Filters;
using System;
using System.Linq;
using Xunit;

namespace ServiceTests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public void AddEmployee_PositiveTest()
        {
            //Arrange
            var employeeService = new EmployeeService();
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77700000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act
            employeeService.AddEmployee(employee);

            //Assert
            Assert.Equal(employee, employeeService.GetEmployee(employee.Id));
        }

        [Fact]
        public void AddEmployee_throwsAgeLimitException()
        {
            //Arrange
            var employeeService = new EmployeeService();
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77700000,
                PassportNumber = 900000000,
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

        [Fact]
        public void GetEmployees_filteredByFirstName()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            employeeService.AddEmployeeList(testDataGenerator.GetEmployeeList(100));
            var filterByFirstName = new EmployeeFilter()
            {
                FirstName = "Ирина"
            };

            //Act
            var filtredEmployees = employeeService.GetEmployees(filterByFirstName);

            //Assert
            Assert.True(filtredEmployees.All(x => x.FirstName.Contains("Ирина")));
        }

        [Fact]
        public void GetEmployees_filteredByLastName()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            employeeService.AddEmployeeList(testDataGenerator.GetEmployeeList(100));
            var filterByLastName = new EmployeeFilter()
            {
                LastName = "Васильев"
            };

            //Act
            var filtredEmployees = employeeService.GetEmployees(filterByLastName);

            //Assert
            Assert.True(filtredEmployees.All(x => x.LastName.Contains("Васильев")));
        }

        [Fact]
        public void GetEmployees_filteredByPhoneNumber()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            employeeService.AddEmployeeList(testDataGenerator.GetEmployeeList(100));
            var filterByPhoneNumber = new EmployeeFilter()
            {
                PhoneNumber = 77700077
            };

            //Act
            var filtredEmployees = employeeService.GetEmployees(filterByPhoneNumber);

            //Assert
            Assert.True(filtredEmployees.All(x => x.PhoneNumber.ToString().Contains(77700077.ToString())));
        }

        [Fact]
        public void GetEmployees_filteredByPassportNumber()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            employeeService.AddEmployeeList(testDataGenerator.GetEmployeeList(100));
            var filterByPassportNumber = new EmployeeFilter()
            {
                PassportNumber = 900000000
            };

            //Act
            var filtredEmployees = employeeService.GetEmployees(filterByPassportNumber);

            //Assert
            Assert.True(filtredEmployees.All(x => x.PassportNumber.ToString().Contains(900000000.ToString())));
        }

        [Fact]
        public void GetEmployees_filteredByDateOfBirth()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            employeeService.AddEmployeeList(testDataGenerator.GetEmployeeList(100));
            var filterByDateOfBirth = new EmployeeFilter()
            {
                MinDate = new DateTime(1990, 1, 1),
                MaxDate = new DateTime(2000, 1, 1)
            };

            //Act
            var filtredEmployees = employeeService.GetEmployees(filterByDateOfBirth);

            //Assert
            Assert.True(filtredEmployees.All(x =>
            x.DateOfBirth >= new DateTime(1990, 1, 1) &&
            x.DateOfBirth <= new DateTime(2000, 1, 1)));
        }

        [Fact]
        public void GetEmployees_filteredWithPagination()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            employeeService.AddEmployeeList(testDataGenerator.GetEmployeeList(100));
            var filterWithPagination = new EmployeeFilter()
            {
                pageNumber = 3,
                notesCount = 10
            };

            //Act
            var filtredEmployees = employeeService.GetEmployees(filterWithPagination);

            //Assert
            Assert.Equal(10, filtredEmployees.Count);
        }

        [Fact]
        public void Delete_PositiveTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77800000,
                PassportNumber = 800000000,
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            employeeService.AddEmployee(employee);

            //Act
            employeeService.DeleteEmployee(employee.Id);

            //Asssert
            Assert.Null(employeeService.GetEmployee(employee.Id));
        }

        [Fact]
        public void Update_PositiveTest()
        {
            //Arrange
            var employeeService = new EmployeeService();
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77800000,
                PassportNumber = 800000002,
                DateOfBirth = new DateTime(2000, 1, 1)
            };
            employeeService.AddEmployee(employee);
            employee.FirstName = "Саша";

            //Act
            employeeService.UpdateEmployee(employee);

            //Assert
            Assert.Equal("Саша", employeeService.GetEmployee(employee.Id).FirstName);
        }
    }
}
