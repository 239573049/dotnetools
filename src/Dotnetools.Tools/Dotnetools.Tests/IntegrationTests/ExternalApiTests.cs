using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnetools.Backend.Models;
using Dotnetools.WebClients.BlockchainInfo;
using Dotnetools.WebClients.Coinbase;
using Dotnetools.WebClients.CoinGecko;
using Dotnetools.WebClients.Bitstamp;
using Dotnetools.WebClients.Gemini;
using Dotnetools.WebClients.ItBit;
using Xunit;
using Dotnetools.Interfaces;
using System.Threading;

namespace Dotnetools.Tests.IntegrationTests;

public class ExternalApiTests
{
	[Fact]
	public async Task CoinbaseExchangeRateProviderTestsAsync() =>
		await AssertProviderAsync(new CoinbaseExchangeRateProvider());

	[Fact]
	public async Task BlockchainInfoExchangeRateProviderTestsAsync() =>
		await AssertProviderAsync(new BlockchainInfoExchangeRateProvider());

	[Fact]
	public async Task CoinGeckoExchangeRateProviderTestsAsync() =>
		await AssertProviderAsync(new CoinGeckoExchangeRateProvider());

	[Fact]
	public async Task BitstampExchangeRateProviderTestsAsync() =>
		await AssertProviderAsync(new BitstampExchangeRateProvider());

	[Fact]
	public async Task GeminiExchangeRateProviderTestsAsync() =>
		await AssertProviderAsync(new GeminiExchangeRateProvider());

	[Fact]
	public async Task ItBitExchangeRateProviderTestsAsync() =>
		await AssertProviderAsync(new ItBitExchangeRateProvider());

	private async Task AssertProviderAsync(IExchangeRateProvider provider)
	{
		using CancellationTokenSource timeoutCts = new(TimeSpan.FromMinutes(3));
		IEnumerable<ExchangeRate> rates = await provider.GetExchangeRateAsync(timeoutCts.Token);

		var usdRate = Assert.Single(rates, x => x.Ticker == "USD");
		Assert.NotEqual(0.0m, usdRate.Rate);
	}
}
