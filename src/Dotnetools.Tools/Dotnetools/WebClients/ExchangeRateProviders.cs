using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnetools.Backend.Models;
using Dotnetools.Interfaces;
using Dotnetools.Logging;
using Dotnetools.WebClients.BlockchainInfo;
using Dotnetools.WebClients.Coinbase;
using Dotnetools.WebClients.Bitstamp;
using Dotnetools.WebClients.CoinGecko;
using Dotnetools.WebClients.Gemini;
using Dotnetools.WebClients.ItBit;
using System.Linq;
using System.Threading;

namespace Dotnetools.WebClients;

public class ExchangeRateProvider : IExchangeRateProvider
{
	private readonly IExchangeRateProvider[] _exchangeRateProviders =
	{
		new BlockchainInfoExchangeRateProvider(),
		new BitstampExchangeRateProvider(),
		new CoinGeckoExchangeRateProvider(),
		new CoinbaseExchangeRateProvider(),
		new GeminiExchangeRateProvider(),
		new ItBitExchangeRateProvider()
	};

	public async Task<IEnumerable<ExchangeRate>> GetExchangeRateAsync(CancellationToken cancellationToken)
	{
		foreach (var provider in _exchangeRateProviders)
		{
			try
			{
				return await provider.GetExchangeRateAsync(cancellationToken).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				// Ignore it and try with the next one
				Logger.LogTrace(ex);
			}
		}
		return Enumerable.Empty<ExchangeRate>();
	}
}
