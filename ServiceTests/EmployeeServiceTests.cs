using Models;
using Services;
using Services.Exceptions;
using Services.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ServiceTests
{
    public class EmployeeServiceTests
    {
        [Fact]
        public async Task AddEmployee_PositiveTestAsync()
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
            await employeeService.AddEmployeeAsync(employee);

            //Assert
            Assert.Equal(employee, await employeeService.GetEmployeeAsync(employee.Id));
        }

        [Fact]
        public async Task AddEmployee_throwsAgeLimitException()
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
            await Assert.ThrowsAsync<AgeLimitException>(async () => await employeeService.AddEmployeeAsync(employee));
        }

        [Fact]
        public async Task AddEmployee_throwsNoPassportDataException()
        {
            //Arrange
            var employeeService = new EmployeeService();
            var employee = new Employee
            {
                DateOfBirth = new DateTime(2000, 1, 1)
            };

            //Act Assert
            await Assert.ThrowsAsync<NoPassportDataException>(async () => await employeeService.AddEmployeeAsync(employee));
        }

        [Fact]
        public async Task GetEmployees_filteredByFirstNameAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            await employeeService.AddEmployeeListAsync(testDataGenerator.GetEmployeeList(100));
            var filterByFirstName = new EmployeeFilter()
            {
                FirstName = "Ирина"
            };

            //Act
            var filtredEmployees = await employeeService.GetEmployeesAsync(filterByFirstName);

            //Assert
            Assert.True(filtredEmployees.All(x => x.FirstName.Contains("Ирина")));
        }

        [Fact]
        public async Task GetEmployees_filteredByLastNameAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            await employeeService.AddEmployeeListAsync(testDataGenerator.GetEmployeeList(100));
            var filterByLastName = new EmployeeFilter()
            {
                LastName = "Васильев"
            };

            //Act
            var filtredEmployees = await employeeService.GetEmployeesAsync(filterByLastName);

            //Assert
            Assert.True(filtredEmployees.All(x => x.LastName.Contains("Васильев")));
        }

        [Fact]
        public async Task GetEmployees_filteredByPhoneNumberAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            await employeeService.AddEmployeeListAsync(testDataGenerator.GetEmployeeList(100));
            var filterByPhoneNumber = new EmployeeFilter()
            {
                PhoneNumber = 77700077
            };

            //Act
            var filtredEmployees = await employeeService.GetEmployeesAsync(filterByPhoneNumber);

            //Assert
            Assert.True(filtredEmployees.All(x => x.PhoneNumber.ToString().Contains(77700077.ToString())));
        }

        [Fact]
        public async Task GetEmployees_filteredByPassportNumberAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            await employeeService.AddEmployeeListAsync(testDataGenerator.GetEmployeeList(100));
            var filterByPassportNumber = new EmployeeFilter()
            {
                PassportNumber = 900000000
            };

            //Act
            var filtredEmployees = await employeeService.GetEmployeesAsync(filterByPassportNumber);

            //Assert
            Assert.True(filtredEmployees.All(x => x.PassportNumber.ToString().Contains(900000000.ToString())));
        }

        [Fact]
        public async Task GetEmployees_filteredByDateOfBirthAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            await employeeService.AddEmployeeListAsync(testDataGenerator.GetEmployeeList(100));
            var filterByDateOfBirth = new EmployeeFilter()
            {
                MinDate = new DateTime(1990, 1, 1),
                MaxDate = new DateTime(2000, 1, 1)
            };

            //Act
            var filtredEmployees = await employeeService.GetEmployeesAsync(filterByDateOfBirth);

            //Assert
            Assert.True(filtredEmployees.All(x =>
            x.DateOfBirth >= new DateTime(1990, 1, 1) &&
            x.DateOfBirth <= new DateTime(2000, 1, 1)));
        }

        [Fact]
        public async Task GetEmployees_filteredWithPaginationAsync()
        {
            //Arrange
            var testDataGenerator = new TestDataGenerator();
            var employeeService = new EmployeeService();
            await employeeService.AddEmployeeListAsync(testDataGenerator.GetEmployeeList(100));
            var filterWithPagination = new EmployeeFilter()
            {
                pageNumber = 3,
                notesCount = 10
            };

            //Act
            var filtredEmployees = await employeeService.GetEmployeesAsync(filterWithPagination);

            //Assert
            Assert.Equal(10, filtredEmployees.Count);
        }

        [Fact]
        public async Task Delete_PositiveTestAsync()
        {
            //Arrange
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
            await employeeService.AddEmployeeAsync(employee);

            //Act
            await employeeService.DeleteEmployeeAsync(employee.Id);

            //Asssert
            Assert.Null(await employeeService.GetEmployeeAsync(employee.Id));
        }

        [Fact]
        public async Task Update_PositiveTestAsync()
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
            await employeeService.AddEmployeeAsync(employee);
            employee.FirstName = "Саша";

            //Act
            await employeeService.UpdateEmployeeAsync(employee);

            //Assert
            var foundEmployees = await employeeService.GetEmployeeAsync(employee.Id);
            Assert.Equal("Саша", foundEmployees.FirstName);
        }
    }
}
