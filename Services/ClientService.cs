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
    public class ClientService
    {
        private ApplicationContextDb _dbContext;
        public ClientService()
        {
            _dbContext = new ApplicationContextDb();
        }

        public Client GetClient(Guid clientId)
        {
            var clientDb = _dbContext.Clients.FirstOrDefault(c => c.Id == clientId);
            var accountCollection = _dbContext.Accounts.Where(x => x.ClientId == clientId).ToList<AccountDb>();
            foreach (var acc in accountCollection)
            {
                clientDb.AccountCollection.Add(acc);
            }

            _dbContext.Entry(clientDb).State = EntityState.Detached;
            foreach (var a in accountCollection)
            {
                _dbContext.Entry(a).State = EntityState.Detached;
            }

            return MapToClient(clientDb);
        }

        public void AddClient(Client client)
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

            _dbContext.Clients.Add(MapToClientDb(client));
            _dbContext.Accounts.Add(accountDb);
            _dbContext.SaveChanges();
        }

        public void AddClientList(List<Client> clientList)
        {
            foreach (var client in clientList)
            {
                AddClient(client);
            }
        }

        public void UpdateClient(Client client)
        {
            var clientDb = _dbContext.Clients.FirstOrDefault(c => c.Id == client.Id);
            if (clientDb == null)
                throw new Exception("Клиента нет в базе");

            _dbContext.Entry(clientDb).State = EntityState.Detached;

            var updatedClientDb = MapToClientDb(client);
            _dbContext.Clients.Update(updatedClientDb);

            _dbContext.SaveChanges();
        }

        public void DeleteClient(Guid clientId)
        {
            _dbContext.ChangeTracker.Clear();

            var client = GetClient(clientId);
            if (client == null)
                throw new Exception("Клиента нет в базе");

            _dbContext.Clients.Remove(MapToClientDb(client));
            _dbContext.SaveChanges();
        }

        public void AddAccount(Guid clientId, Account account)
        {
            account.ClientId = clientId;
            _dbContext.Accounts.Add(MapToAccountDb(account));
            _dbContext.SaveChanges();
        }

        public void UpdateAccount(Account account)
        {
            var accountDb = _dbContext.Accounts.FirstOrDefault(c => c.Id == account.Id);
            if (accountDb == null)
                throw new Exception("Аккаунта нет в базе");

            _dbContext.Entry(accountDb).State = EntityState.Detached;

            var updatedAccountDb = MapToAccountDb(account);
            _dbContext.Accounts.Update(updatedAccountDb);

            _dbContext.SaveChanges();
        }
            
        public void DeleteAccount(Guid accountId)
        {
            var dbAccount = _dbContext.Accounts.AsTracking().FirstOrDefault(c => c.Id == accountId);
            if (dbAccount == null)
                throw new Exception("Аккаунта нет в базе");

            _dbContext.Accounts.Remove(dbAccount);
            _dbContext.SaveChanges();
        }

        public List<Client> GetClients(ClientFilter filter)
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

            List<Client> list = new List<Client>();
            foreach (var item in query.ToList())
            {
                list.Add(GetClient(item.Id));
            }

            return list;
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
