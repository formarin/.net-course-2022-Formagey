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
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        ClientService _clientService;

        public ClientController(ILogger<ClientController> logger)
        {
            _logger = logger;
            _clientService = new ClientService();
        }

        [HttpGet]
        public async Task<Client> GetAsync(Guid Id)
        {
            return await _clientService.GetClientAsync(Id);
        }

        [HttpPost]
        public async Task AddAsync(Client client)
        {
            await _clientService.AddClientAsync(client);
        }

        [HttpPut]
        public async Task UpdateAsync(Client client)
        {
            await _clientService.UpdateClientAsync(client);
        }

        [HttpDelete]
        public async Task DeleteAsync(Guid Id)
        {
            await _clientService.DeleteClientAsync(Id);
        }
    }
}
