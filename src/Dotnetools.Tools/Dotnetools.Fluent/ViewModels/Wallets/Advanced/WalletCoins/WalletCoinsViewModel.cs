using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Controls.Templates;
using DynamicData;
using ReactiveUI;
using Dotnetools.Blockchain.Analysis.Clustering;
using Dotnetools.Blockchain.TransactionOutputs;
using Dotnetools.Fluent.Extensions;
using Dotnetools.Fluent.ViewModels.Dialogs;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Fluent.ViewModels.Wallets.Send;
using Dotnetools.Fluent.Views.Wallets.Advanced.WalletCoins.Columns;

namespace Dotnetools.Fluent.ViewModels.Wallets.Advanced.WalletCoins;

[NavigationMetaData(Title = "Wallet Coins (UTXOs)")]
public partial class WalletCoinsViewModel : RoutableViewModel
{
	private readonly WalletViewModel _walletVm;
	[AutoNotify] private IObservable<bool> _isAnySelected = Observable.Return(false);

	[AutoNotify]
	private FlatTreeDataGridSource<WalletCoinViewModel> _source = new(Enumerable.Empty<WalletCoinViewModel>());

	public WalletCoinsViewModel(WalletViewModel walletVm)
	{
		_walletVm = walletVm;
		SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

		NextCommand = CancelCommand;
		SkipCommand = ReactiveCommand.CreateFromTask(OnSendCoinsAsync);
	}

	protected override void OnNavigatedTo(bool isInHistory, CompositeDisposable disposables)
	{
		var coins = CreateCoinsObservable(_walletVm.UiTriggers.TransactionsUpdateTrigger);

		var coinChanges = coins
			.ToObservableChangeSet(c => c.HdPubKey.GetHashCode())
			.AsObservableCache()
			.Connect()
			.TransformWithInlineUpdate(x => new WalletCoinViewModel(x))
			.Replay(1)
			.RefCount();

		IsAnySelected = coinChanges
			.AutoRefresh(x => x.IsSelected)
			.ToCollection()
			.Select(items => items.Any(t => t.IsSelected))
			.ObserveOn(RxApp.MainThreadScheduler);

		coinChanges
			.DisposeMany()
			.ObserveOn(RxApp.MainThreadScheduler)
			.Bind(out var coinsCollection)
			.Subscribe()
			.DisposeWith(disposables);

		Source = CreateGridSource(coinsCollection)
			.DisposeWith(disposables);

		base.OnNavigatedTo(isInHistory, disposables);
	}

	private static int GetOrderingPriority(WalletCoinViewModel x)
	{
		if (x.CoinJoinInProgress)
		{
			return 1;
		}

		if (x.IsBanned)
		{
			return 2;
		}

		if (!x.Confirmed)
		{
			return 3;
		}

		return 0;
	}

	private IObservable<ICoinsView> CreateCoinsObservable(IObservable<Unit> balanceChanged)
	{
		var initial = Observable.Return(GetCoins());
		var coinJoinChanged = _walletVm.WhenAnyValue(model => model.IsCoinJoining);
		var coinsChanged = balanceChanged.ToSignal().Merge(coinJoinChanged.ToSignal());

		var coins = coinsChanged
			.Select(_ => GetCoins());

		var concat = initial.Concat(coins);
		return concat;
	}

	private async Task OnSendCoinsAsync()
	{
		var wallet = _walletVm.Wallet;
		var selectedSmartCoins = Source.Items.Where(x => x.IsSelected).Select(x => x.Coin).ToImmutableArray();

		var addressDialog = new AddressEntryDialogViewModel(wallet.Network);
		var addressResult = await NavigateDialogAsync(addressDialog, NavigationTarget.CompactDialogScreen);
		if (addressResult.Result is not { } address || address.Address is null)
		{
			return;
		}

		var labelDialog = new LabelEntryDialogViewModel(wallet, address.Label ?? SmartLabel.Empty);
		var result = await NavigateDialogAsync(labelDialog, NavigationTarget.CompactDialogScreen);
		if (result.Result is not { } label)
		{
			return;
		}

		var info = new TransactionInfo(address.Address, wallet.AnonScoreTarget)
		{
			Coins = selectedSmartCoins,
			Amount = selectedSmartCoins.Sum(x => x.Amount),
			SubtractFee = true,
			Recipient = label,
			IsSelectedCoinModificationEnabled = false,
			IsFixedAmount = true,
		};

		Navigate().To(new TransactionPreviewViewModel(wallet, info));
	}

	private FlatTreeDataGridSource<WalletCoinViewModel> CreateGridSource(IEnumerable<WalletCoinViewModel> coins)
	{
		// [Column]			[View]					[Header]	[Width]		[MinWidth]		[MaxWidth]	[CanUserSort]
		// Selection		SelectionColumnView		-			Auto		-				-			false
		// Indicators		IndicatorsColumnView	-			Auto		-				-			true
		// Amount			AmountColumnView		Amount		Auto		-				-			true
		// AnonymityScore	AnonymityColumnView		<custom>	50			-				-			true
		// Labels			LabelsColumnView		Labels		*			-				-			true
		var source = new FlatTreeDataGridSource<WalletCoinViewModel>(coins)
		{
			Columns =
			{
				// Selection
				new TemplateColumn<WalletCoinViewModel>(
					null,
					new FuncDataTemplate<WalletCoinViewModel>((node, ns) => new SelectionColumnView(), true),
					options: new ColumnOptions<WalletCoinViewModel>
					{
						CanUserResizeColumn = false,
						CanUserSortColumn = false
					},
					width: new GridLength(0, GridUnitType.Auto)),

				// Indicators
				new TemplateColumn<WalletCoinViewModel>(
					null,
					new FuncDataTemplate<WalletCoinViewModel>((node, ns) => new IndicatorsColumnView(), true),
					options: new ColumnOptions<WalletCoinViewModel>
					{
						CanUserResizeColumn = false,
						CanUserSortColumn = true,
						CompareAscending = WalletCoinViewModel.SortAscending(x => GetOrderingPriority(x)),
						CompareDescending = WalletCoinViewModel.SortDescending(x => GetOrderingPriority(x))
					},
					width: new GridLength(0, GridUnitType.Auto)),

				// Amount
				new TemplateColumn<WalletCoinViewModel>(
					"Amount",
					new FuncDataTemplate<WalletCoinViewModel>((node, ns) => new AmountColumnView(), true),
					options: new ColumnOptions<WalletCoinViewModel>
					{
						CanUserResizeColumn = false,
						CanUserSortColumn = true,
						CompareAscending = WalletCoinViewModel.SortAscending(x => x.Amount),
						CompareDescending = WalletCoinViewModel.SortDescending(x => x.Amount)
					},
					width: new GridLength(0, GridUnitType.Auto)),

				// AnonymityScore
				new TemplateColumn<WalletCoinViewModel>(
					new AnonymitySetHeaderView(),
					new FuncDataTemplate<WalletCoinViewModel>((node, ns) => new AnonymitySetColumnView(), true),
					options: new ColumnOptions<WalletCoinViewModel>
					{
						CanUserResizeColumn = false,
						CanUserSortColumn = true,
						CompareAscending = WalletCoinViewModel.SortAscending(x => x.AnonymitySet),
						CompareDescending = WalletCoinViewModel.SortDescending(x => x.AnonymitySet)
					},
					width: new GridLength(50, GridUnitType.Pixel)),

				// Labels
				new TemplateColumn<WalletCoinViewModel>(
					"Labels",
					new FuncDataTemplate<WalletCoinViewModel>((node, ns) => new LabelsColumnView(), true),
					options: new ColumnOptions<WalletCoinViewModel>
					{
						CanUserResizeColumn = false,
						CanUserSortColumn = true,
						CompareAscending = WalletCoinViewModel.SortAscending(x => x.SmartLabel),
						CompareDescending = WalletCoinViewModel.SortDescending(x => x.SmartLabel)
					},
					width: new GridLength(1, GridUnitType.Star))
			}
		};

		source.RowSelection!.SingleSelect = true;

		return source;
	}

	private ICoinsView GetCoins()
	{
		return _walletVm.Wallet.Coins;
	}
}