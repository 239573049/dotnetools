using System.Reactive.Disposables;

namespace Dotnetools.Fluent.ViewModels;

public class ActivatableViewModel : ViewModelBase
{
	protected virtual void OnActivated(CompositeDisposable disposables)
	{
	}

	public void Activate(CompositeDisposable disposables)
	{
		OnActivated(disposables);
	}
}
