using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using Dotnetools.Blockchain.Keys;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Wallets;

namespace Dotnetools.Fluent.ViewModels.Wallets;

public partial class WalletSettingsViewModel : RoutableViewModel
{
	private readonly Wallet _wallet;
	[AutoNotify] private string _plebStopThreshold;
	[AutoNotify] private bool _preferPsbtWorkflow;

	public WalletSettingsViewModel(WalletViewModelBase walletViewModelBase)
	{
		_wallet = walletViewModelBase.Wallet;
		Title = $"{_wallet.WalletName} - Wallet Settings";
		_preferPsbtWorkflow = _wallet.KeyManager.PreferPsbtWorkflow;
		IsHardwareWallet = _wallet.KeyManager.IsHardwareWallet;
		IsWatchOnly = _wallet.KeyManager.IsWatchOnly;
		_plebStopThreshold = _wallet.KeyManager.PlebStopThreshold?.ToString() ??
							 KeyManager.DefaultPlebStopThreshold.ToString();

		SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

		NextCommand = CancelCommand;

		VerifyRecoveryWordsCommand =
			ReactiveCommand.Create(() => Navigate().To(new VerifyRecoveryWordsViewModel(_wallet)));

		this.WhenAnyValue(x => x.PreferPsbtWorkflow)
			.Skip(1)
			.Subscribe(
				value =>
				{
					_wallet.KeyManager.PreferPsbtWorkflow = value;
					_wallet.KeyManager.ToFile();
					walletViewModelBase.RaisePropertyChanged(nameof(walletViewModelBase.PreferPsbtWorkflow));
				});
	}

	public bool IsHardwareWallet { get; }

	public bool IsWatchOnly { get; }

	public override sealed string Title { get; protected set; }

	public ICommand VerifyRecoveryWordsCommand { get; }
}
