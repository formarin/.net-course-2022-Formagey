using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Services;
using System;
using System.Threading.Tasks;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        EmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
            _employeeService = new EmployeeService();
        }

        [HttpGet]
        public async Task<Employee> GetAsync(Guid Id)
        {
            return await _employeeService.GetEmployeeAsync(Id);
        }

        [HttpPost]
        public async Task AddAsync(Employee employee)
        {
            await _employeeService.AddEmployeeAsync(employee);
        }

        [HttpPut]
        public async Task UpdateAsync(Employee employee)
        {
            await _employeeService.UpdateEmployeeAsync(employee);
        }

        [HttpDelete]
        public async Task DeleteAsync(Guid Id)
        {
            await _employeeService.DeleteEmployeeAsync(Id);
        }
    }
}
