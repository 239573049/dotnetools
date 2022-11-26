using System.Windows.Input;
using ReactiveUI;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Wallets.PasswordFinder;

namespace Dotnetools.Fluent.ViewModels.Login.PasswordFinder;

[NavigationMetaData(Title = "Password Finder")]
public partial class ContainsNumbersViewModel : RoutableViewModel
{
	public ContainsNumbersViewModel(PasswordFinderOptions options)
	{
		Options = options;

		SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

		EnableBack = true;

		YesCommand = ReactiveCommand.Create(() => SetAnswer(true));
		NoCommand = ReactiveCommand.Create(() => SetAnswer(false));
	}

	public PasswordFinderOptions Options { get; }

	public ICommand YesCommand { get; }

	public ICommand NoCommand { get; }

	private void SetAnswer(bool ans)
	{
		Options.UseNumbers = ans;
		Navigate().To(new ContainsSymbolsViewModel(Options));
	}
}
