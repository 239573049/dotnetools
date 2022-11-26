using Dotnetools.WabiSabi.Client.CoinJoinProgressEvents;
using Dotnetools.Wallets;

namespace Dotnetools.WabiSabi.Client.StatusChangedEvents;

public class CoinJoinStatusEventArgs : StatusChangedEventArgs
{
	public CoinJoinStatusEventArgs(IWallet wallet, CoinJoinProgressEventArgs coinJoinProgressEventArgs) : base(wallet)
	{
		CoinJoinProgressEventArgs = coinJoinProgressEventArgs;
	}

	public CoinJoinProgressEventArgs CoinJoinProgressEventArgs { get; }
}
