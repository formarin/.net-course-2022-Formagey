using Models;
using System;
using System.Threading.Tasks;

namespace Services
{
    public class CashDispenserService
    {
        public Task CashOut(double amount, Account account)
        {
            if (account.Amount < amount)
                throw new Exception("Недостаточно стредств");

            return new Task(() =>
            {
                account.Amount -= amount;

                var clientService = new ClientService();

                Task.Delay(7000);

                clientService.UpdateAccount(account);
            });
        }
    }
}
