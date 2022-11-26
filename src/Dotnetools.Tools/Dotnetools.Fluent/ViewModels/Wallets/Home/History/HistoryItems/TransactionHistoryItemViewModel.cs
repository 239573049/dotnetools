using System.Reactive.Linq;
using NBitcoin;
using ReactiveUI;
using Dotnetools.Blockchain.Transactions;
using Dotnetools.Fluent.Extensions;
using Dotnetools.Fluent.Helpers;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Fluent.ViewModels.Wallets.Home.History.Details;

namespace Dotnetools.Fluent.ViewModels.Wallets.Home.History.HistoryItems;

public class TransactionHistoryItemViewModel : HistoryItemViewModelBase
{
	public TransactionHistoryItemViewModel(
		int orderIndex,
		TransactionSummary transactionSummary,
		WalletViewModel walletVm,
		Money balance)
		: base(orderIndex, transactionSummary)
	{
		Label = transactionSummary.Label;
		IsConfirmed = transactionSummary.IsConfirmed();
		Date = transactionSummary.DateTime.ToLocalTime();
		Balance = balance;

		var confirmations = transactionSummary.GetConfirmations();
		ConfirmedToolTip = $"{confirmations} confirmation{TextHelpers.AddSIfPlural(confirmations)}";

		var amount = transactionSummary.Amount;
		if (amount < Money.Zero)
		{
			OutgoingAmount = amount * -1;
		}
		else
		{
			IncomingAmount = amount;
		}

		ShowDetailsCommand = ReactiveCommand.Create(() =>
			RoutableViewModel.Navigate(NavigationTarget.DialogScreen).To(
				new TransactionDetailsViewModel(transactionSummary, walletVm)));

		var speedUpTransactionCommandCanExecute = this.WhenAnyValue(x => x.IsConfirmed)
			.Select(x => !x)
			.ObserveOn(RxApp.MainThreadScheduler);

		SpeedUpTransactionCommand = ReactiveCommand.Create(
			() =>
			{
				// TODO: Show speed up transaction dialog.
			},
			speedUpTransactionCommandCanExecute);

		DateString = $"{Date.ToLocalTime():MM/dd/yyyy HH:mm}";
	}
}