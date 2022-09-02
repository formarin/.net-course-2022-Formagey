namespace Models
{
    public class Client : Person
    {
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
            return ((Client)obj).PhoneNumber == PhoneNumber;
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
            return PhoneNumber.GetHashCode();
        }
    }
}
