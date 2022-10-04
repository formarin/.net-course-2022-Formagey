using System.Collections.Generic;

namespace Models
{
    public class Client : Person
    {
        public ICollection<Account> AccountCollection { get; set; } = new List<Account>();
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Client))
            {
                return false;
            }

            var client = obj as Client;
            return client.Id == Id &
                client.FirstName == FirstName &
                client.LastName == LastName &
                client.PassportNumber == PassportNumber &
                client.PhoneNumber == PhoneNumber &
                client.DateOfBirth.Year == DateOfBirth.Year;
        }
        public static bool operator ==(Client client1, Client client2)
        {
            return client1.Equals(client2);
        }
        public static bool operator !=(Client client1, Client client2)
        {
            return !client1.Equals(client2);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
