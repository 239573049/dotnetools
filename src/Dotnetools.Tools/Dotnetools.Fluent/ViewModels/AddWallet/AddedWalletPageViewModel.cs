using System.Linq;
using System.Reactive.Disposables;
using ReactiveUI;
using Dotnetools.Blockchain.Keys;
using Dotnetools.Fluent.Helpers;
using Dotnetools.Fluent.ViewModels.NavBar;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Wallets;

namespace Dotnetools.Fluent.ViewModels.AddWallet;

[NavigationMetaData(Title = "Success")]
public partial class AddedWalletPageViewModel : RoutableViewModel
{
	private readonly KeyManager _keyManager;

	public AddedWalletPageViewModel(KeyManager keyManager)
	{
		_keyManager = keyManager;
		WalletName = _keyManager.WalletName;
		WalletType = WalletHelpers.GetType(_keyManager);

		SetupCancel(enableCancel: false, enableCancelOnEscape: false, enableCancelOnPressed: false);
		EnableBack = false;

		NextCommand = ReactiveCommand.Create(OnNext);
	}

	public WalletType WalletType { get; }

	public string WalletName { get; }

	private void OnNext()
	{
		Navigate().Clear();

		var navBar = NavigationManager.Get<NavBarViewModel>();

		var wallet = navBar?.Wallets.FirstOrDefault(x => x.WalletName == WalletName);

		if (wallet is { } && navBar is { })
		{
			navBar.SelectedItem = wallet;
			wallet.OpenCommand.Execute(default);
		}
	}

	protected override void OnNavigatedTo(bool isInHistory, CompositeDisposable disposables)
	{
		base.OnNavigatedTo(isInHistory, disposables);

		if (!Services.WalletManager.WalletExists(_keyManager.MasterFingerprint))
		{
			Services.WalletManager.AddWallet(_keyManager);
		}
	}
}
