using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Threading;
using NBitcoin;
using NBitcoin.Payment;
using ReactiveUI;
using Dotnetools.Blockchain.Analysis.Clustering;
using Dotnetools.Fluent.Models;
using Dotnetools.Fluent.Validation;
using Dotnetools.Fluent.ViewModels.Dialogs;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Fluent.ViewModels.Wallets.Home.Tiles;
using Dotnetools.Logging;
using Dotnetools.Models;
using Dotnetools.Tor.Http;
using Dotnetools.Tor.Socks5.Pool.Circuits;
using Dotnetools.Userfacing;
using Dotnetools.WabiSabi.Client;
using Dotnetools.Wallets;
using Dotnetools.WebClients.PayJoin;
using Constants = Dotnetools.Helpers.Constants;

namespace Dotnetools.Fluent.ViewModels.Wallets.Send;

[NavigationMetaData(
	Title = "Send",
	Caption = "",
	IconName = "wallet_action_send",
	NavBarPosition = NavBarPosition.None,
	Searchable = false,
	NavigationTarget = NavigationTarget.DialogScreen)]
public partial class SendViewModel : RoutableViewModel
{
	private readonly object _parsingLock = new();
	private readonly Wallet _wallet;
	private readonly CoinJoinManager? _coinJoinManager;
	private bool _parsingTo;
	private SmartLabel _parsedLabel = SmartLabel.Empty;
	[AutoNotify] private string _to;
	[AutoNotify] private decimal _amountBtc;
	[AutoNotify] private decimal _exchangeRate;
	[AutoNotify] private bool _isFixedAmount;
	[AutoNotify] private bool _isPayJoin;
	[AutoNotify] private string? _payJoinEndPoint;
	[AutoNotify] private bool _conversionReversed;

	public SendViewModel(WalletViewModel walletVm)
	{
		_to = "";
		_wallet = walletVm.Wallet;
		_coinJoinManager = Services.HostedServices.GetOrDefault<CoinJoinManager>();

		_conversionReversed = Services.UiConfig.SendAmountConversionReversed;

		IsQrButtonVisible = WebcamQrReader.IsOsPlatformSupported;

		ExchangeRate = _wallet.Synchronizer.UsdExchangeRate;

		Balance = new WalletBalanceTileViewModel(walletVm);

		SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

		EnableBack = false;

		this.ValidateProperty(x => x.To, ValidateToField);
		this.ValidateProperty(x => x.AmountBtc, ValidateAmount);

		this.WhenAnyValue(x => x.To)
			.Skip(1)
			.Subscribe(ParseToField);

		this.WhenAnyValue(x => x.PayJoinEndPoint)
			.Subscribe(endPoint => IsPayJoin = endPoint is { });

		PasteCommand = ReactiveCommand.CreateFromTask(async () => await OnPasteAsync());
		AutoPasteCommand = ReactiveCommand.CreateFromTask(async () => await OnAutoPasteAsync());
		InsertMaxCommand = ReactiveCommand.Create(() => AmountBtc = _wallet.Coins.TotalAmount().ToDecimal(MoneyUnit.BTC));
		QrCommand = ReactiveCommand.Create(async () =>
		{
			ShowQrCameraDialogViewModel dialog = new(_wallet.Network);
			var result = await NavigateDialogAsync(dialog, NavigationTarget.CompactDialogScreen);
			if (!string.IsNullOrWhiteSpace(result.Result))
			{
				To = result.Result;
			}
		});

		var nextCommandCanExecute =
			this.WhenAnyValue(x => x.AmountBtc, x => x.To)
				.Select(tup =>
				{
					var (amountBtc, to) = tup;
					var allFilled = !string.IsNullOrEmpty(to) && amountBtc > 0;
					var hasError = Validations.Any;

					return allFilled && !hasError;
				});

		NextCommand = ReactiveCommand.CreateFromTask(async () =>
		{
			var labelDialog = new LabelEntryDialogViewModel(_wallet, _parsedLabel);
			var result = await NavigateDialogAsync(labelDialog, NavigationTarget.CompactDialogScreen);
			if (result.Result is not { } label)
			{
				return;
			}

			var transactionInfo = new TransactionInfo(BitcoinAddress.Create(To, _wallet.Network), _wallet.AnonScoreTarget)
			{
				Amount = new Money(AmountBtc, MoneyUnit.BTC),
				Recipient = label,
				PayJoinClient = PayJoinEndPoint is { } ? GetPayjoinClient(PayJoinEndPoint) : null,
				IsFixedAmount = _isFixedAmount
			};

			Navigate().To(new TransactionPreviewViewModel(_wallet, transactionInfo));
		}, nextCommandCanExecute);

		this.WhenAnyValue(x => x.ConversionReversed)
			.Skip(1)
			.Subscribe(x => Services.UiConfig.SendAmountConversionReversed = x);
	}

	public bool IsQrButtonVisible { get; }

	public ICommand PasteCommand { get; }

	public ICommand AutoPasteCommand { get; }

	public ICommand QrCommand { get; }

	public ICommand InsertMaxCommand { get; }

	public WalletBalanceTileViewModel Balance { get; }

	private async Task OnAutoPasteAsync()
	{
		var isAutoPasteEnabled = Services.UiConfig.AutoPaste;

		if (string.IsNullOrEmpty(To) && isAutoPasteEnabled)
		{
			await OnPasteAsync(pasteIfInvalid: false);
		}
	}

	private async Task OnPasteAsync(bool pasteIfInvalid = true)
	{
		if (Application.Current is { Clipboard: { } clipboard })
		{
			var text = await clipboard.GetTextAsync();

			lock (_parsingLock)
			{
				if (!TryParseUrl(text) && pasteIfInvalid)
				{
					To = text;
				}
			}
		}
	}

	private IPayjoinClient? GetPayjoinClient(string endPoint)
	{
		if (!string.IsNullOrWhiteSpace(endPoint) &&
			Uri.IsWellFormedUriString(endPoint, UriKind.Absolute))
		{
			var payjoinEndPointUri = new Uri(endPoint);
			if (!Services.Config.UseTor)
			{
				if (payjoinEndPointUri.DnsSafeHost.EndsWith(".onion", StringComparison.OrdinalIgnoreCase))
				{
					Logger.LogWarning("Payjoin server is an onion service but Tor is disabled. Ignoring...");
					return null;
				}

				if (Services.Config.Network == Network.Main && payjoinEndPointUri.Scheme != Uri.UriSchemeHttps)
				{
					Logger.LogWarning("Payjoin server is not exposed as an onion service nor https. Ignoring...");
					return null;
				}
			}

			IHttpClient httpClient = Services.HttpClientFactory.NewHttpClient(() => payjoinEndPointUri, Mode.DefaultCircuit);
			return new PayjoinClient(payjoinEndPointUri, httpClient);
		}

		return null;
	}

	private void ValidateAmount(IValidationErrors errors)
	{
		if (AmountBtc > Constants.MaximumNumberOfBitcoins)
		{
			errors.Add(ErrorSeverity.Error, "Amount must be less than the total supply of BTC.");
		}
		else if (AmountBtc > _wallet.Coins.TotalAmount().ToDecimal(MoneyUnit.BTC))
		{
			errors.Add(ErrorSeverity.Error, "Insufficient funds to cover the amount requested.");
		}
		else if (AmountBtc <= 0)
		{
			errors.Add(ErrorSeverity.Error, "Amount must be more than 0 BTC");
		}
	}

	private void ValidateToField(IValidationErrors errors)
	{
		if (!string.IsNullOrEmpty(To) && (To.IsTrimmable() || !AddressStringParser.TryParse(To, _wallet.Network, out _)))
		{
			errors.Add(ErrorSeverity.Error, "Input a valid BTC address or URL.");
		}
		else if (IsPayJoin && _wallet.KeyManager.IsHardwareWallet)
		{
			errors.Add(ErrorSeverity.Error, "Payjoin is not possible with hardware wallets.");
		}
	}

	private void ParseToField(string s)
	{
		lock (_parsingLock)
		{
			Dispatcher.UIThread.Post(() => TryParseUrl(s));
		}
	}

	private bool TryParseUrl(string? text)
	{
		if (_parsingTo)
		{
			return false;
		}

		_parsingTo = true;

		text = text?.Trim();

		if (string.IsNullOrEmpty(text))
		{
			_parsingTo = false;
			PayJoinEndPoint = null;
			IsFixedAmount = false;
			return false;
		}

		bool result = false;

		if (AddressStringParser.TryParse(text, _wallet.Network, out BitcoinUrlBuilder? url))
		{
			result = true;
			if (url.Label is { } label)
			{
				_parsedLabel = new SmartLabel(label);
			}
			else
			{
				_parsedLabel = SmartLabel.Empty;
			}

			if (url.UnknownParameters.TryGetValue("pj", out var endPoint))
			{
				PayJoinEndPoint = endPoint;
			}
			else
			{
				PayJoinEndPoint = null;
			}

			if (url.Address is { })
			{
				To = url.Address.ToString();
			}

			if (url.Amount is { })
			{
				AmountBtc = url.Amount.ToDecimal(MoneyUnit.BTC);
				IsFixedAmount = true;
			}
			else
			{
				IsFixedAmount = false;
			}
		}
		else
		{
			IsFixedAmount = false;
			PayJoinEndPoint = null;
			_parsedLabel = SmartLabel.Empty;
		}

		Dispatcher.UIThread.Post(() => _parsingTo = false);

		return result;
	}

	protected override void OnNavigatedTo(bool inHistory, CompositeDisposable disposables)
	{
		if (!inHistory)
		{
			To = "";
			AmountBtc = 0;
			ClearValidations();

			if (_coinJoinManager is { } coinJoinManager)
			{
				coinJoinManager.IsUserInSendWorkflow = true;
			}
		}

		_wallet.Synchronizer.WhenAnyValue(x => x.UsdExchangeRate)
			.ObserveOn(RxApp.MainThreadScheduler)
			.Subscribe(x => ExchangeRate = x)
			.DisposeWith(disposables);

		RxApp.MainThreadScheduler.Schedule(async () => await OnAutoPasteAsync());

		Balance.Activate(disposables);

		base.OnNavigatedTo(inHistory, disposables);
	}

	protected override void OnNavigatedFrom(bool isInHistory)
	{
		base.OnNavigatedFrom(isInHistory);

		if (!isInHistory && _coinJoinManager is { } coinJoinManager)
		{
			coinJoinManager.IsUserInSendWorkflow = false;
		}
	}
}