using Dotnetools.Wallets;

namespace Dotnetools.Fluent.ViewModels.Wallets;

public class WatchOnlyWalletViewModel : WalletViewModel
{
	internal WatchOnlyWalletViewModel(Wallet wallet)
		: base(wallet)
	{
	}
}
