using Dotnetools.Blockchain.Analysis.Clustering;
using Dotnetools.Blockchain.TransactionOutputs;
using Dotnetools.Helpers;
using Dotnetools.Models;

namespace Dotnetools.Fluent.Helpers;

public static class CoinHelpers
{
	public static bool IsPrivate(this SmartCoin coin, int privateThreshold)
	{
		return coin.HdPubKey.AnonymitySet >= privateThreshold;
	}

	public static bool IsSemiPrivate(this SmartCoin coin, int privateThreshold)
	{
		var anonymitySet = coin.HdPubKey.AnonymitySet;
		return anonymitySet >= Constants.SemiPrivateThreshold && anonymitySet < privateThreshold;
	}

	public static SmartLabel GetLabels(this SmartCoin coin, int privateThreshold)
	{
		if (coin.IsPrivate(privateThreshold) || coin.IsSemiPrivate(privateThreshold))
		{
			return SmartLabel.Empty;
		}

		if (coin.HdPubKey.Cluster.Labels == SmartLabel.Empty)
		{
			return CoinPocketHelper.UnlabelledFundsText;
		}

		return coin.HdPubKey.Cluster.Labels;
	}

	public static int GetConfirmations(this SmartCoin coin) => coin.Height.Type == HeightType.Chain ? (int)Services.BitcoinStore.SmartHeaderChain.TipHeight - coin.Height.Value + 1 : 0;
}
