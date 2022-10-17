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
    public class ClientService
    {
        private ApplicationContextDb _dbContext;
        public ClientService()
        {
            _dbContext = new ApplicationContextDb();
        }

        public async Task<Client> GetClientAsync(Guid clientId)
        {
            var clientDb = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            
            if (clientDb == null)
                return null;

            clientDb.AccountCollection = await _dbContext.Accounts.Where(x => x.ClientId == clientId).ToListAsync<AccountDb>();

            _dbContext.Entry(clientDb).State = EntityState.Detached;
            foreach (var account in clientDb.AccountCollection)
            {
                _dbContext.Entry(account).State = EntityState.Detached;
            }

            return MapToClient(clientDb);
        }

        public async Task AddClientAsync(Client client)
        {
            if (client.FirstName == null | client.LastName == null | client.LastName == null |
                client.PassportNumber == 0 | client.DateOfBirth == new DateTime(0))
            {
                throw new NoPassportDataException("Наличие всех паспортных данных обязательно.");
            }
            if (DateTime.Now.Year - client.DateOfBirth.Year < 18)
            {
                throw new AgeLimitException("Минимально допустимый возраст: 18 лет.");
            }

            var accountDb = new AccountDb
            {
                Amount = 0,
                CurrencyName = "USD",
                ClientId = client.Id
            };

            await _dbContext.Clients.AddAsync(MapToClientDb(client));
            await _dbContext.Accounts.AddAsync(accountDb);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddClientListAsync(List<Client> clientList)
        {
            foreach (var client in clientList)
            {
                await AddClientAsync(client);
            }
        }

        public async Task UpdateClientAsync(Client client)
        {
            var clientDb = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == client.Id);
            if (clientDb == null)
                throw new Exception("Клиента нет в базе");

            _dbContext.Entry(clientDb).State = EntityState.Detached;

            var updatedClientDb = MapToClientDb(client);
            _dbContext.Clients.Update(updatedClientDb);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(Guid clientId)
        {
            var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
            if (client == null)
                throw new Exception("Клиента нет в базе");

            _dbContext.Entry(client).State = EntityState.Detached;

            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddAccountAsync(Account account)
        {
            await _dbContext.Accounts.AddAsync(MapToAccountDb(account));
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(Account account)
        {
            var accountDb = await _dbContext.Accounts.FirstOrDefaultAsync(c => c.Id == account.Id);
            if (accountDb == null)
                throw new Exception("Аккаунта нет в базе");

            _dbContext.Entry(accountDb).State = EntityState.Detached;

            var updatedAccountDb = MapToAccountDb(account);
            _dbContext.Accounts.Update(updatedAccountDb);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(Guid accountId)
        {
            var dbAccount = await _dbContext.Accounts.AsTracking().FirstOrDefaultAsync(c => c.Id == accountId);
            if (dbAccount == null)
                throw new Exception("Аккаунта нет в базе");

            _dbContext.Accounts.Remove(dbAccount);
            await _dbContext.SaveChangesAsync(); ;
        }

        public async Task<List<Client>> GetClientsAsync(ClientFilter filter)
        {
            var query = _dbContext.Clients.AsQueryable();

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

            var clientList = new List<Client>();
            var clientDbList = await query.ToListAsync();
            foreach (var item in clientDbList)
            {
                clientList.Add(await GetClientAsync(item.Id));
            }

            return clientList;
        }

        private Client MapToClient(ClientDb clientDb)
        {
            return new Mapper(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<AccountDb, Account>();
                    cfg.CreateMap<ClientDb, Client>();
                })).Map<Client>(clientDb);
        }

        private ClientDb MapToClientDb(Client client)
        {
            return new Mapper(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Account, AccountDb>();
                    cfg.CreateMap<Client, ClientDb>();
                })).Map<ClientDb>(client);
        }

        private AccountDb MapToAccountDb(Account account)
        {
            return new Mapper(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Account, AccountDb>();
                })).Map<AccountDb>(account);
        }
    }
}
