using ReactiveUI;
using Dotnetools.Blockchain.Transactions;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Wallets;

namespace Dotnetools.Fluent.ViewModels.Wallets.Send;

[NavigationMetaData(Title = "Payment successful")]
public partial class SendSuccessViewModel : RoutableViewModel
{
	private readonly Wallet _wallet;
	private readonly SmartTransaction _finalTransaction;

	public SendSuccessViewModel(Wallet wallet, SmartTransaction finalTransaction)
	{
		_wallet = wallet;
		_finalTransaction = finalTransaction;

		NextCommand = ReactiveCommand.Create(OnNext);

		SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);
	}

	private void OnNext()
	{
		Navigate().Clear();

		var walletViewModel = UiServices.WalletManager.GetWalletViewModel(_wallet);

		walletViewModel.History.SelectTransaction(_finalTransaction.GetHash());
	}
}
