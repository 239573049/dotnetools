using System.Collections.Generic;
using System.Linq;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Wallets.PasswordFinder;

namespace Dotnetools.Fluent.ViewModels.Login.PasswordFinder;

[NavigationMetaData(Title = "Password Finder")]
public partial class SelectCharsetViewModel : RoutableViewModel
{
	public SelectCharsetViewModel(PasswordFinderOptions options)
	{
		Options = options;
		SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

		EnableBack = true;

		Charsets = Enum.GetValues(typeof(Charset)).Cast<Charset>().Select(x => new CharsetViewModel(this, x));
	}

	public PasswordFinderOptions Options { get; }

	public IEnumerable<CharsetViewModel> Charsets { get; }
}
