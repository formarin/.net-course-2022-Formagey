using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Models;
using ModelsDb;
using Services.Exceptions;
using Services.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class EmployeeService
    {
        private ApplicationContextDb _dbContext;
        public EmployeeService()
        {
            _dbContext = new ApplicationContextDb();
        }

        public async Task<Employee> GetEmployeeAsync(Guid employeeId)
        {
            var employeeDb = await _dbContext.Employees.FirstOrDefaultAsync(c => c.Id == employeeId);

            if (employeeDb == null)
                return null;

            return MapToEmployee(employeeDb);
        }

        public async Task AddEmployeeAsync(Employee employee)
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

            await _dbContext.Employees.AddAsync(MapToEmployeeDb(employee));
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddEmployeeListAsync(List<Employee> employeeList)
        {
            foreach (var employee in employeeList)
            {
                await AddEmployeeAsync(employee);
            }
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            var employeeDb = await _dbContext.Employees.FirstOrDefaultAsync(c => c.Id == employee.Id);
            if (employeeDb == null)
                throw new Exception("Сотрудника нет в базе");

            _dbContext.Entry(employeeDb).State = EntityState.Detached;

            var updatedEmployeeDb = MapToEmployeeDb(employee);
            _dbContext.Employees.Update(updatedEmployeeDb);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Guid employeeId)
        {
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(c => c.Id == employeeId);
            if (employee == null)
                throw new Exception("Сотрудника нет в базе");

            _dbContext.Entry(employee).State = EntityState.Detached;

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetEmployeesAsync(EmployeeFilter filter)
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

            var employeeList = new List<Employee>();
            List<EmployeeDb> employeeDbList = await query.ToListAsync();
            foreach (var item in employeeDbList)
                employeeList.Add(MapToEmployee(item));

            return employeeList;
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
