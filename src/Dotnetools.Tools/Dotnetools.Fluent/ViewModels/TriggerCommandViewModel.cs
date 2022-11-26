using System.Windows.Input;
using Dotnetools.Fluent.ViewModels.Navigation;

namespace Dotnetools.Fluent.ViewModels;

public abstract class TriggerCommandViewModel : RoutableViewModel
{
	public abstract ICommand TargetCommand { get; }
}
