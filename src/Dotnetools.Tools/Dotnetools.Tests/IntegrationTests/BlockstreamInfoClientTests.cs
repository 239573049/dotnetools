using NBitcoin;
using System.Threading;
using System.Threading.Tasks;
using Dotnetools.Blockchain.Analysis.FeesEstimation;
using Dotnetools.Tests.Helpers;
using Dotnetools.Tor;
using Dotnetools.WebClients.BlockstreamInfo;
using Dotnetools.WebClients.Wasabi;
using Xunit;

namespace Dotnetools.Tests.IntegrationTests;

public class BlockstreamInfoClientTests : IAsyncLifetime
{
	public BlockstreamInfoClientTests()
	{
		ClearnetHttpClientFactory = new(torEndPoint: null, backendUriGetter: null);
		TorHttpClientFactory = new(Common.TorSocks5Endpoint, backendUriGetter: null);

		TorManager = new(Common.TorSettings);
	}

	private HttpClientFactory ClearnetHttpClientFactory { get; }
	private HttpClientFactory TorHttpClientFactory { get; }
	private TorProcessManager TorManager { get; }

	public async Task InitializeAsync()
	{
		await TorManager.StartAsync();
	}

	public async Task DisposeAsync()
	{
		await ClearnetHttpClientFactory.DisposeAsync();
		await TorHttpClientFactory.DisposeAsync();
		await TorManager.DisposeAsync();
	}

	[Fact]
	public async Task GetFeeEstimatesClearnetMainnetAsync()
	{
		BlockstreamInfoClient client = new(Network.Main, ClearnetHttpClientFactory);
		AllFeeEstimate estimates = await client.GetFeeEstimatesAsync(CancellationToken.None);
		Assert.NotNull(estimates);
		Assert.NotEmpty(estimates.Estimations);
	}

	[Fact]
	public async Task GetFeeEstimatesTorMainnetAsync()
	{
		BlockstreamInfoClient client = new(Network.Main, TorHttpClientFactory);
		AllFeeEstimate estimates = await client.GetFeeEstimatesAsync(CancellationToken.None);
		Assert.NotNull(estimates);
		Assert.NotEmpty(estimates.Estimations);
	}

	[Fact]
	public async Task GetFeeEstimatesClearnetTestnetAsync()
	{
		BlockstreamInfoClient client = new(Network.TestNet, ClearnetHttpClientFactory);
		AllFeeEstimate estimates = await client.GetFeeEstimatesAsync(CancellationToken.None);
		Assert.NotNull(estimates);
		Assert.NotEmpty(estimates.Estimations);
	}

	[Fact]
	public async Task GetFeeEstimatesTorTestnetAsync()
	{
		BlockstreamInfoClient client = new(Network.TestNet, TorHttpClientFactory);
		AllFeeEstimate estimates = await client.GetFeeEstimatesAsync(CancellationToken.None);
		Assert.NotNull(estimates);
		Assert.NotEmpty(estimates.Estimations);
	}

	[Fact]
	public async Task SimulatesFeeEstimatesClearnetRegtestAsync()
	{
		BlockstreamInfoClient client = new(Network.RegTest, ClearnetHttpClientFactory);
		AllFeeEstimate estimates = await client.GetFeeEstimatesAsync(CancellationToken.None);
		Assert.NotNull(estimates);
		Assert.NotEmpty(estimates.Estimations);
	}

	[Fact]
	public async Task SimulatesFeeEstimatesTorRegtestAsync()
	{
		BlockstreamInfoClient client = new(Network.RegTest, TorHttpClientFactory);
		AllFeeEstimate estimates = await client.GetFeeEstimatesAsync(CancellationToken.None);
		Assert.NotNull(estimates);
		Assert.NotEmpty(estimates.Estimations);
	}
}