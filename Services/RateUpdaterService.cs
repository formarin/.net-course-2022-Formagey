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
            Task.Run(async () =>
            {
                var clientService = new ClientService();
                var clients = await clientService.GetClientsAsync(new Filters.ClientFilter());

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
                        await clientService.UpdateAccountAsync(accounts[i]);
                    }
                    await Task.Delay(3000);
                }
            }, token);
            return Task.CompletedTask;
        }
    }
}
