using Dotnetools.Fluent.Models;
using Dotnetools.Fluent.ViewModels.Dialogs.Authorization;
using Dotnetools.Wallets;

namespace Dotnetools.Fluent.Helpers;

public static class AuthorizationHelpers
{
	public static AuthorizationDialogBase GetAuthorizationDialog(Wallet wallet, TransactionAuthorizationInfo transactionAuthorizationInfo)
	{
		if (wallet.KeyManager.IsHardwareWallet)
		{
			return new HardwareWalletAuthDialogViewModel(wallet, transactionAuthorizationInfo);
		}
		else
		{
			return new PasswordAuthDialogViewModel(wallet);
		}
	}
}
