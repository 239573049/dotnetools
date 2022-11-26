using Dotnetools.Wallets;

namespace Dotnetools.WabiSabi.Client.StatusChangedEvents;

public class StartErrorEventArgs : StatusChangedEventArgs
{
	public StartErrorEventArgs(IWallet wallet, CoinjoinError error)
		: base(wallet)
	{
		Error = error;
	}

	public CoinjoinError Error { get; }
}
