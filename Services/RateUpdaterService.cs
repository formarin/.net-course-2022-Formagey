using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public class RateUpdaterService
    {
        public Task RateUpdater(CancellationToken token)
        {
            return new Task(() =>
            {
                var clientService = new ClientService();
                var clients = clientService.GetClients(new Filters.ClientFilter());

                var accounts = new List<Account>();
                foreach (var client in clients)
                {
                    accounts.Add(client.AccountCollection.FirstOrDefault());
                };

                while (!token.IsCancellationRequested)
                {
                    for (var i = 0; i < accounts.Count; i++)
                    {
                        accounts[i].Amount += 10;
                        clientService.UpdateAccount(accounts[i]);
                    }
                    Task.Delay(3000).Wait();
                }
            });
        }
    }
}
