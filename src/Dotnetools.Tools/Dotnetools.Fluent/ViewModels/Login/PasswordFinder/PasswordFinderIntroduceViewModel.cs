using System.Threading.Tasks;
using ReactiveUI;
using Dotnetools.Fluent.ViewModels.Dialogs;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Wallets;
using Dotnetools.Wallets.PasswordFinder;

namespace Dotnetools.Fluent.ViewModels.Login.PasswordFinder;

[NavigationMetaData(Title = "Password Finder")]
public partial class PasswordFinderIntroduceViewModel : RoutableViewModel
{
	public PasswordFinderIntroduceViewModel(Wallet wallet)
	{
		SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

		EnableBack = false;

		NextCommand = ReactiveCommand.CreateFromTask(async () => await OnNextAsync(wallet));
	}

	private async Task OnNextAsync(Wallet wallet)
	{
		var dialogResult =
			await NavigateDialogAsync(
				new CreatePasswordDialogViewModel("Password", "Type in your most likely password", enableEmpty: false)
				, NavigationTarget.CompactDialogScreen);

		if (dialogResult.Result is { } password)
		{
			var options = new PasswordFinderOptions(wallet, password);
			Navigate().To(new SelectCharsetViewModel(options));
		}
	}
}
