using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Dotnetools.Backend.Models;
using Dotnetools.Interfaces;
using Dotnetools.Tor.Http.Extensions;

namespace Dotnetools.WebClients.Coinbase;

public class CoinbaseExchangeRateProvider : IExchangeRateProvider
{
	public async Task<IEnumerable<ExchangeRate>> GetExchangeRateAsync(CancellationToken cancellationToken)
	{
		using var httpClient = new HttpClient
		{
			BaseAddress = new Uri("https://api.coinbase.com")
		};
		using var response = await httpClient.GetAsync("/v2/exchange-rates?currency=BTC", cancellationToken).ConfigureAwait(false);
		using var content = response.Content;
		var wrapper = await content.ReadAsJsonAsync<DataWrapper>().ConfigureAwait(false);

		var exchangeRates = new List<ExchangeRate>
		{
			new ExchangeRate { Rate = wrapper.Data.Rates.USD, Ticker = "USD" }
		};

		return exchangeRates;
	}

	private class DataWrapper
	{
		public CoinbaseExchangeRate Data { get; set; }

		public class CoinbaseExchangeRate
		{
			public ExchangeRates Rates { get; set; }

			public class ExchangeRates
			{
				public decimal USD { get; set; }
			}
		}
	}
}