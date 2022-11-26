using NBitcoin;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dotnetools.Blockchain.TransactionOutputs;
using Dotnetools.Extensions;
using Dotnetools.WabiSabi.Client.RoundStateAwaiters;
using Dotnetools.Wallets;
using Dotnetools.WebClients.Wasabi;

namespace Dotnetools.WabiSabi.Client;

public class CoinJoinTrackerFactory
{
	public CoinJoinTrackerFactory(
		IWasabiHttpClientFactory httpClientFactory,
		RoundStateUpdater roundStatusUpdater,
		string coordinatorIdentifier,
		CancellationToken cancellationToken)
	{
		HttpClientFactory = httpClientFactory;
		RoundStatusUpdater = roundStatusUpdater;
		CoordinatorIdentifier = coordinatorIdentifier;
		CancellationToken = cancellationToken;
		LiquidityClueProvider = new LiquidityClueProvider();
	}

	private IWasabiHttpClientFactory HttpClientFactory { get; }
	private RoundStateUpdater RoundStatusUpdater { get; }
	private CancellationToken CancellationToken { get; }
	private string CoordinatorIdentifier { get; }
	private LiquidityClueProvider LiquidityClueProvider { get; }

	public async Task<CoinJoinTracker> CreateAndStartAsync(IWallet wallet, IEnumerable<SmartCoin> coinCandidates, bool restartAutomatically, bool overridePlebStop)
	{
		await LiquidityClueProvider.InitLiquidityClueAsync(wallet, CancellationToken).ConfigureAwait(false);

		if (wallet.KeyChain is null)
		{
			throw new NotSupportedException("Wallet has no key chain.");
		}

		var coinJoinClient = new CoinJoinClient(
			HttpClientFactory,
			wallet.KeyChain,
			wallet.DestinationProvider,
			RoundStatusUpdater,
			CoordinatorIdentifier,
			LiquidityClueProvider,
			wallet.AnonScoreTarget,
			consolidationMode: wallet.ConsolidationMode,
			redCoinIsolation: wallet.RedCoinIsolation,
			feeRateMedianTimeFrame: wallet.FeeRateMedianTimeFrame,
			doNotRegisterInLastMinuteTimeLimit: TimeSpan.FromMinutes(1));

		return new CoinJoinTracker(wallet, coinJoinClient, coinCandidates, restartAutomatically, overridePlebStop, CancellationToken);
	}
}
