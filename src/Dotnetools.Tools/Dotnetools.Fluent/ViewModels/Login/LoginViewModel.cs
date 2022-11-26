using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using Dotnetools.Fluent.Helpers;
using Dotnetools.Fluent.ViewModels.AddWallet;
using Dotnetools.Fluent.ViewModels.Login.PasswordFinder;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Fluent.ViewModels.Wallets;
using Dotnetools.Userfacing;
using Dotnetools.Wallets;

namespace Dotnetools.Fluent.ViewModels.Login;

[NavigationMetaData(Title = "")]
public partial class LoginViewModel : RoutableViewModel
{
	[AutoNotify] private string _password;
	[AutoNotify] private bool _isPasswordNeeded;
	[AutoNotify] private string _errorMessage;
	[AutoNotify] private bool _isForgotPasswordVisible;

	public LoginViewModel(ClosedWalletViewModel closedWalletViewModel)
	{
		var wallet = closedWalletViewModel.Wallet;
		IsPasswordNeeded = !wallet.KeyManager.IsWatchOnly;
		WalletName = wallet.WalletName;
		_password = "";
		_errorMessage = "";
		WalletType = WalletHelpers.GetType(closedWalletViewModel.Wallet.KeyManager);

		NextCommand = ReactiveCommand.CreateFromTask(async () => await OnNextAsync(closedWalletViewModel, wallet));

		OkCommand = ReactiveCommand.Create(OnOk);

		ForgotPasswordCommand = ReactiveCommand.Create(() => OnForgotPassword(wallet));

		EnableAutoBusyOn(NextCommand);
	}

	public WalletType WalletType { get; }

	public string WalletName { get; }

	public ICommand OkCommand { get; }

	public ICommand ForgotPasswordCommand { get; }

	private async Task OnNextAsync(ClosedWalletViewModel closedWalletViewModel, Wallet wallet)
	{
		string? compatibilityPasswordUsed = null;

		var isPasswordCorrect = await Task.Run(() => wallet.TryLogin(Password, out compatibilityPasswordUsed));

		if (!isPasswordCorrect)
		{
			IsForgotPasswordVisible = true;
			ErrorMessage = "The password is incorrect! Please try again.";
			return;
		}

		if (compatibilityPasswordUsed is { })
		{
			await ShowErrorAsync(Title, PasswordHelper.CompatibilityPasswordWarnMessage, "Compatibility password was used");
		}

		var legalResult = await ShowLegalAsync();

		if (legalResult)
		{
			LoginWallet(closedWalletViewModel);
		}
		else
		{
			wallet.Logout();
			ErrorMessage = "You must accept the Terms and Conditions!";
		}
	}

	private void OnOk()
	{
		Password = "";
		ErrorMessage = "";
	}

	private void OnForgotPassword(Wallet wallet)
	{
		Navigate(NavigationTarget.DialogScreen).To(new PasswordFinderIntroduceViewModel(wallet));
	}

	private void LoginWallet(ClosedWalletViewModel closedWalletViewModel)
	{
		closedWalletViewModel.RaisePropertyChanged(nameof(WalletViewModelBase.IsLoggedIn));
		Navigate().To(closedWalletViewModel, NavigationMode.Clear);
	}

	private async Task<bool> ShowLegalAsync()
	{
		if (!Services.LegalChecker.TryGetNewLegalDocs(out _))
		{
			return true;
		}

		var legalDocs = new TermsAndConditionsViewModel();

		var dialogResult = await NavigateDialogAsync(legalDocs, NavigationTarget.DialogScreen);

		if (dialogResult.Result)
		{
			await Services.LegalChecker.AgreeAsync();
		}

		return dialogResult.Result;
	}
}
