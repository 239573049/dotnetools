using Dotnetools.Fluent.ViewModels.NavBar;

namespace Dotnetools.Fluent.ViewModels.Dialogs.Base;

/// <summary>
/// CommonBase class.
/// </summary>
public abstract partial class DialogViewModelBase : NavBarItemViewModel
{
	[AutoNotify] private bool _isDialogOpen;
}
