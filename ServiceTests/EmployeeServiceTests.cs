using Models;
using Services;
using Services.Exceptions;
using Services.Filters;
using Services.Storages;
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
            var employeeStorage = new EmployeeStorage();
            var employeeService = new EmployeeService<IEmployeeStorage>(employeeStorage);
            var employee = new Employee
            {
                FirstName = "firstName",
                LastName = "lastName",
                PhoneNumber = 77700000,
                PassportNumber = 900000000,
                DateOfBirth = new DateTime(2000, 1, 1),
                Salary = 500
            };

            //Act
            employeeService.AddEmployee(employee);

            //Assert
            Assert.Contains(employee, employeeStorage.Data);
        }

        [Fact]
        public void AddEmployee_throwsAgeLimitException()
        {
            //Arrange
            var employeeService = new EmployeeService<IEmployeeStorage>(new EmployeeStorage());
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
            var employeeService = new EmployeeService<IEmployeeStorage>(new EmployeeStorage());
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
            var employeeStorage = new EmployeeStorage();
            employeeStorage.Add(testDataGenerator.GetEmployeeList(1000));
            var employeeService = new EmployeeService<IEmployeeStorage>(employeeStorage);
            var filterByFirstName = new EmployeeFilter()
            {
                FirstName = "Ирина"
            };

            //Act
            var filtredEmployeeList = employeeService.GetEmployees(filterByFirstName);

            //Assert
            Assert.True(filtredEmployeeList.All(x => x.FirstName == "Ирина"));
        }

        [Fact]
        public void GetEmployees_filteredByLastName()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeStorage = new EmployeeStorage();
            employeeStorage.Add(testDataGenerator.GetEmployeeList(1000));
            var employeeService = new EmployeeService<IEmployeeStorage>(employeeStorage);
            var filterByLastName = new EmployeeFilter()
            {
                LastName = "Васильев"
            };

            //Act
            var filtredEmployeeList = employeeService.GetEmployees(filterByLastName);

            //Assert
            Assert.True(filtredEmployeeList.All(x => x.LastName == "Васильев"));
        }

        [Fact]
        public void GetEmployees_filteredByPhoneNumber()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeStorage = new EmployeeStorage();
            employeeStorage.Add(testDataGenerator.GetEmployeeList(1000));

            var employeeService = new EmployeeService<IEmployeeStorage>(employeeStorage);
            var filterByPhoneNumber = new EmployeeFilter()
            {
                PhoneNumber = 77700077
            };

            //Act
            var filtredEmployeeList = employeeService.GetEmployees(filterByPhoneNumber);

            //Assert
            Assert.True(filtredEmployeeList.All(x => x.PhoneNumber == 77700077));
        }

        [Fact]
        public void GetEmployees_filteredByPassportNumber()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeStorage = new EmployeeStorage();
            employeeStorage.Add(testDataGenerator.GetEmployeeList(1000));

            var employeeService = new EmployeeService<IEmployeeStorage>(employeeStorage);
            var filterByPassportNumber = new EmployeeFilter()
            {
                PassportNumber = 900000000
            };

            //Act
            var filtredEmployeeList = employeeService.GetEmployees(filterByPassportNumber);

            //Assert
            Assert.True(filtredEmployeeList.All(x => x.PassportNumber == 900000000));
        }

        [Fact]
        public void GetEmployees_filteredByDateOfBirth()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeStorage = new EmployeeStorage();
            employeeStorage.Add(testDataGenerator.GetEmployeeList(1000));

            var employeeService = new EmployeeService<IEmployeeStorage>(employeeStorage);
            var filterByDateOfBirth = new EmployeeFilter()
            {
                MinDate = new DateTime(1990, 1, 1),
                MaxDate = new DateTime(2000, 1, 1)
            };

            //Act
            var filtredEmployeeList = employeeService.GetEmployees(filterByDateOfBirth);

            //Assert
            Assert.True(filtredEmployeeList.All(x =>
            x.DateOfBirth >= new DateTime(1990, 1, 1) &&
            x.DateOfBirth <= new DateTime(2000, 1, 1)));
        }

        [Fact]
        public void Delete_PositiveTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeStorage = new EmployeeStorage();
            employeeStorage.Add(testDataGenerator.GetEmployeeList(5));
            var employeeService = new EmployeeService<IEmployeeStorage>(employeeStorage);
            var employee = employeeStorage.Data.First();

            //Act
            employeeService.DeleteEmployee(employee);

            //Asssert
            Assert.DoesNotContain(employee, employeeStorage.Data);
        }

        [Fact]
        public void Update_PositiveTest()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeStorage = new EmployeeStorage();
            employeeStorage.Add(testDataGenerator.GetEmployeeList(5));
            var employeeService = new EmployeeService<IEmployeeStorage>(employeeStorage);
            var employee = employeeStorage.Data.First();
            var employeePassport = employee.PassportNumber;

            //Act
            employee.LastName = "lastName2";
            employeeService.UpdateEmployee(employee);

            //Assert
            Assert.Equal("lastName2", employeeStorage.Data.Find(x => x.PassportNumber == employeePassport).LastName);
        }
    }
}
