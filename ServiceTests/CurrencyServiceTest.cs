using Services;
using Xunit;

namespace ServiceTests
{
    public class CurrencyServiceTest
    {
        [Fact]
        public async void ExchangeMoneyAsync_PositiveTest()
        {
            var currencyService = new CurrencyService();
            float amount = await currencyService.ExchangeMoneyAsync("T8WpWqBUumdqf5Bz8Bw6DJPfdRQBwn", "USD", "EUR", 50);
        }
    }
}
