using Models;
using System;
using System.Threading.Tasks;

namespace Services
{
    public class CashDispenserService
    {
        public async Task CashOut(double amount, Account account)
        {
            if (account.Amount < amount)
                throw new Exception("Недостаточно стредств");

             await Task.Run(async () =>
            {
                account.Amount -= amount;

                var clientService = new ClientService();

                await clientService.UpdateAccountAsync(account);

                await Task.Delay(1000);
            });
        }
    }
}
