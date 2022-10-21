using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CurrencyService
    {
        public async Task<float> ExchangeMoneyAsync(string api_key, string currencyFrom, string currencyTo, float currencyAmount)
        {
            CurrencyApiResponse currencyApiResponse;

            using (var client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync($"https://www.amdoren.com/api/currency.php?" +
                    $"api_key={api_key}&" +
                    $"from={currencyFrom}&" +
                    $"to={currencyTo}&" +
                    $"amount={currencyAmount}");

                responseMessage.EnsureSuccessStatusCode();

                var message = await responseMessage.Content.ReadAsStringAsync();
                currencyApiResponse = JsonConvert.DeserializeObject<CurrencyApiResponse>(message);
            }

            return currencyApiResponse.amount;
        }
    }
}
