using Dotnetools.Wallets;

namespace Dotnetools.WabiSabi.Client.StatusChangedEvents;

public class WalletStartedCoinJoinEventArgs : StatusChangedEventArgs
{
	public WalletStartedCoinJoinEventArgs(IWallet wallet) : base(wallet)
	{
	}
}
