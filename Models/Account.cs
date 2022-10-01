using System;

namespace Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public string CurrencyName { get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Account))
            {
                return false;
            }

            var account = obj as Account;
            return account.ClientId == ClientId &
                account.Amount == Amount &
                account.CurrencyName == CurrencyName;
        }
        public static bool operator ==(Account account1, Account account2)
        {
            return account1.Equals(account2);
        }
        public static bool operator !=(Account account1, Account account2)
        {
            return !account1.Equals(account2);
        }
    }
}
