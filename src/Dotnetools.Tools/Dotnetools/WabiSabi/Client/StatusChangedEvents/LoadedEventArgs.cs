using Dotnetools.Wallets;

namespace Dotnetools.WabiSabi.Client.StatusChangedEvents;

public class LoadedEventArgs : StatusChangedEventArgs
{
	public LoadedEventArgs(IWallet wallet)
		: base(wallet)
	{
	}
}
