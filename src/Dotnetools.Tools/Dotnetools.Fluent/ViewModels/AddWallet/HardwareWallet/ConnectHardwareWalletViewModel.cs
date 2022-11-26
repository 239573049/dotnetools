using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using Dotnetools.Fluent.ViewModels.NavBar;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Fluent.ViewModels.Wallets;
using Dotnetools.Helpers;
using Dotnetools.Hwi.Models;
using Dotnetools.Logging;
using Dotnetools.Nito.AsyncEx;
using Dotnetools.Wallets;

namespace Dotnetools.Fluent.ViewModels.AddWallet.HardwareWallet;

[NavigationMetaData(Title = "Hardware Wallet")]
public partial class ConnectHardwareWalletViewModel : RoutableViewModel
{
	[AutoNotify] private string _message;
	[AutoNotify] private bool _isSearching;
	[AutoNotify] private bool _existingWalletFound;
	[AutoNotify] private bool _confirmationRequired;

	public ConnectHardwareWalletViewModel(string walletName)
	{
		_message = "";
		WalletName = walletName;
		Wallets = UiServices.WalletManager.Wallets;
		AbandonedTasks = new AbandonedTasks();
		CancelCts = new CancellationTokenSource();

		EnableBack = true;

		NextCommand = ReactiveCommand.Create(OnNext);

		NavigateToExistingWalletLoginCommand = ReactiveCommand.Create(execute: OnNavigateToExistingWalletLogin);

		this.WhenAnyValue(x => x.Message)
			.ObserveOn(RxApp.MainThreadScheduler)
			.Subscribe(message => ConfirmationRequired = !string.IsNullOrEmpty(message));
	}

	private HwiEnumerateEntry? DetectedDevice { get; set; }

	public CancellationTokenSource CancelCts { get; set; }

	private AbandonedTasks AbandonedTasks { get; }

	public string WalletName { get; }

	public ObservableCollection<WalletViewModelBase> Wallets { get; }

	public WalletViewModelBase? ExistingWallet { get; set; }

	public ICommand NavigateToExistingWalletLoginCommand { get; }

	public WalletType Ledger => WalletType.Ledger;

	public WalletType Coldcard => WalletType.Coldcard;

	public WalletType Trezor => WalletType.Trezor;

	public WalletType Generic => WalletType.Hardware;

	private void OnNext()
	{
		if (DetectedDevice is { } device)
		{
			NavigateToNext(device);
			return;
		}

		StartDetection();
	}

	private void OnNavigateToExistingWalletLogin()
	{
		var navBar = NavigationManager.Get<NavBarViewModel>();

		if (ExistingWallet is { } && navBar is { })
		{
			navBar.SelectedItem = ExistingWallet;
			Navigate().Clear();
			ExistingWallet.OpenCommand.Execute(default);
		}
	}

	private void StartDetection()
	{
		Message = "";

		if (IsSearching)
		{
			return;
		}

		DetectedDevice = null;
		ExistingWalletFound = false;
		AbandonedTasks.AddAndClearCompleted(DetectionAsync(CancelCts.Token));
	}

	private async Task DetectionAsync(CancellationToken cancel)
	{
		IsSearching = true;

		try
		{
			using CancellationTokenSource cts = new();
			AbandonedTasks.AddAndClearCompleted(CheckForPassphraseAsync(cts.Token));
			var result = await HardwareWalletOperationHelpers.DetectAsync(Services.WalletManager.Network, cancel);
			cts.Cancel();
			EvaluateDetectionResult(result, cancel);
		}
		catch (Exception ex) when (ex is not OperationCanceledException)
		{
			Logger.LogError(ex);
		}
		finally
		{
			IsSearching = false;
		}
	}

	private async Task CheckForPassphraseAsync(CancellationToken cancellationToken)
	{
		try
		{
			await Task.Delay(7000, cancellationToken);
			Message = "Check your device and enter your passphrase, then click Rescan.";
		}
		catch (OperationCanceledException)
		{
			// ignored
		}
	}

	private void EvaluateDetectionResult(HwiEnumerateEntry[] devices, CancellationToken cancel)
	{
		if (devices.Length == 0)
		{
			Message = "Connect the hardware wallet to the PC / Enter the PIN on the device.";
			return;
		}

		if (devices.Length > 1)
		{
			Message = "Make sure you have only one hardware wallet connected to the PC.";
			return;
		}

		var device = devices[0];

		if (Services.WalletManager.WalletExists(device.Fingerprint))
		{
			ExistingWallet = Wallets.FirstOrDefault(x => x.Wallet.KeyManager.MasterFingerprint == device.Fingerprint);
			Message = "The connected hardware wallet is already added to the software, click below to open it or click Rescan to search again.";
			ExistingWalletFound = true;
			return;
		}

		if (!device.IsInitialized())
		{
			if (device.Model == HardwareWalletModels.Coldcard)
			{
				Message = "Initialize your device first.";
			}
			else
			{
				Message = "Check your device and finish the initialization.";
				AbandonedTasks.AddAndClearCompleted(HardwareWalletOperationHelpers.InitHardwareWalletAsync(device, Services.WalletManager.Network, cancel));
			}

			return;
		}

		if (device.Code is { })
		{
			Message = "Something happened with your device, unlock it with your PIN/Passphrase or reconnect to the PC.";
			return;
		}

		if (device.NeedsPassphraseSent == true)
		{
			Message = "Enter your passphrase on your device.";
			return;
		}

		if (device.NeedsPinSent == true)
		{
			Message = "Enter your PIN on your device.";
			return;
		}

		DetectedDevice = device;

		if (!ConfirmationRequired)
		{
			NavigateToNext(DetectedDevice);
		}
	}

	private void NavigateToNext(HwiEnumerateEntry device)
	{
		Navigate().To(new DetectedHardwareWalletViewModel(WalletName, device));
	}

	protected override void OnNavigatedTo(bool isInHistory, CompositeDisposable disposables)
	{
		base.OnNavigatedTo(isInHistory, disposables);

		var enableCancel = Services.WalletManager.HasWallet();

		SetupCancel(enableCancel: enableCancel, enableCancelOnEscape: enableCancel, enableCancelOnPressed: false);

		if (isInHistory)
		{
			CancelCts = new CancellationTokenSource();
		}

		StartDetection();

		disposables.Add(Disposable.Create(async () =>
		{
			CancelCts.Cancel();
			await AbandonedTasks.WhenAllAsync();
			CancelCts.Dispose();
		}));
	}
}