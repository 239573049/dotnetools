using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using Dotnetools.Blockchain.Analysis.Clustering;
using Dotnetools.Fluent.ViewModels.Dialogs.Base;
using Dotnetools.Fluent.ViewModels.Wallets.Labels;
using Dotnetools.Fluent.ViewModels.Wallets.Send;
using Dotnetools.Wallets;

namespace Dotnetools.Fluent.ViewModels.Dialogs;

[NavigationMetaData(Title = "Recipient")]
public partial class LabelEntryDialogViewModel : DialogViewModelBase<SmartLabel?>
{
	private readonly Wallet _wallet;

	public LabelEntryDialogViewModel(Wallet wallet, SmartLabel label)
	{
		_wallet = wallet;
		SuggestionLabels = new SuggestionLabelsViewModel(wallet.KeyManager, Intent.Send, 3)
		{
			Labels = { label.Labels }
		};

		SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

		var nextCommandCanExecute =
			Observable
				.Merge(SuggestionLabels.WhenAnyValue(x => x.Labels.Count).Select(_ => Unit.Default))
				.Merge(SuggestionLabels.WhenAnyValue(x => x.IsCurrentTextValid).Select(_ => Unit.Default))
				.Select(_ => SuggestionLabels.Labels.Any() || SuggestionLabels.IsCurrentTextValid);

		NextCommand = ReactiveCommand.Create(OnNext, nextCommandCanExecute);
	}

	public SuggestionLabelsViewModel SuggestionLabels { get; }

	private void OnNext()
	{
		Close(DialogResultKind.Normal, new SmartLabel(SuggestionLabels.Labels.ToArray()));
	}

	protected override void OnNavigatedTo(bool isInHistory, CompositeDisposable disposables)
	{
		base.OnNavigatedTo(isInHistory, disposables);

		_wallet.TransactionProcessor.WhenAnyValue(x => x.Coins)
			.Select(_ => Unit.Default)
			.ObserveOn(RxApp.MainThreadScheduler)
			.Subscribe(_ => SuggestionLabels.UpdateLabels())
			.DisposeWith(disposables);
	}
}
