namespace Models
{
    public class Account
    {
        public Currency Currency { get; set; } =
            new Currency
            {
                Code = 840,
                Name = "USD"
            };
        public double Amount { get; set; }
    }
}
