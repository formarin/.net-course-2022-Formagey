using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using ModelsDb;
using Services.Exceptions;
using Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class EmployeeService
    {
        private ApplicationContextDb _dbContext;
        public EmployeeService()
        {
            _dbContext = new ApplicationContextDb();
        }

        public Employee GetEmployee(Guid employeeId)
        {
            var employeeDb = _dbContext.Employees.AsNoTracking().FirstOrDefault(c => c.Id == employeeId);
            return MapToEmployee(employeeDb);
        }

        public void AddEmployee(Employee employee)
        {
            if (employee.FirstName == null | employee.LastName == null | employee.LastName == null |
                employee.PassportNumber == 0 | employee.DateOfBirth == new DateTime(0))
            {
                throw new NoPassportDataException("Наличие всех паспортных данных обязательно.");
            }
            if (DateTime.Now.Year - employee.DateOfBirth.Year < 18)
            {
                throw new AgeLimitException("Минимально допустимый возраст: 18 лет.");
            }

            _dbContext.Employees.Add(MapToEmployeeDb(employee));
            _dbContext.SaveChanges();
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
            if (GetEmployee(employee.Id) == null)
                throw new Exception("Сотрудника нет в базе");

            var dbEmployee = _dbContext.Employees.AsTracking().FirstOrDefault(c => c.Id == employee.Id);
            var mappedEmployee = MapToEmployeeDb(employee);

            MakeShallowCopy(mappedEmployee, ref dbEmployee);
            _dbContext.SaveChanges();
        }

        private void MakeShallowCopy(EmployeeDb employeeDb, ref EmployeeDb outEmployeeDb)
        {
            var employeeType = employeeDb.GetType();

            foreach (var property in employeeType.GetProperties())
            {
                if (property.Name.Contains("Id"))
                    continue;
                var svoystvoDb = employeeType.GetProperty(property.Name);
                svoystvoDb.SetValue(outEmployeeDb, svoystvoDb.GetValue(employeeDb));
            }
        }

        public void DeleteEmployee(Guid employeeId)
        {
            _dbContext.ChangeTracker.Clear();

            var employee = GetEmployee(employeeId);
            if (employee == null)
                throw new Exception("Сотрудника нет в базе");

            _dbContext.Employees.Remove(MapToEmployeeDb(employee));
            _dbContext.SaveChanges();
        }

        public List<Employee> GetEmployees(EmployeeFilter filter)
        {
            var query = _dbContext.Employees.Where(_ => true);

            if (filter.FirstName != null)
                query = query.Where(x => x.FirstName.Contains(filter.FirstName));

            if (filter.LastName != null)
                query = query.Where(x => x.LastName.Contains(filter.LastName));

            if (filter.PhoneNumber != null)
                query = query.Where(x => x.PhoneNumber.ToString().Contains(filter.PhoneNumber.ToString()));

            if (filter.PassportNumber != null)
                query = query.Where(x => x.PassportNumber.ToString().Contains(filter.PassportNumber.ToString()));

            if (filter.MinDate != null)
                query = query.Where(x => x.DateOfBirth >= filter.MinDate);

            if (filter.MaxDate != null)
                query = query.Where(x => x.DateOfBirth <= filter.MaxDate);

            if (filter.pageNumber != null & filter.notesCount != null)
                query = query.Skip(filter.notesCount.Value * (filter.pageNumber.Value - 1)).Take(filter.notesCount.Value);

            List<Employee> list = new List<Employee>();
            foreach (var item in query)
                list.Add(MapToEmployee(item));

            return list;
        }

        private Employee MapToEmployee(EmployeeDb employeeDb)
        {
            return new Mapper(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<EmployeeDb, Employee>();
                })).Map<Employee>(employeeDb);
        }

        private EmployeeDb MapToEmployeeDb(Employee employee)
        {
            return new Mapper(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Employee, EmployeeDb>();
                })).Map<EmployeeDb>(employee);
        }
    }
}
