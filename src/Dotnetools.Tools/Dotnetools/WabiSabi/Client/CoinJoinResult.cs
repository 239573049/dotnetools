using NBitcoin;
using System.Collections.Immutable;
using Dotnetools.Blockchain.TransactionOutputs;

namespace Dotnetools.WabiSabi.Client;

public record CoinJoinResult(
	bool GoForBlameRound,
	bool SuccessfulBroadcast,
	ImmutableList<SmartCoin> RegisteredCoins,
	ImmutableList<Script> RegisteredOutputs)
{
	public CoinJoinResult(bool goForBlameRound) :
		this(goForBlameRound, false, ImmutableList<SmartCoin>.Empty, ImmutableList<Script>.Empty)
	{
	}
}
