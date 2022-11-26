using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;
using Dotnetools.Fluent.Helpers;
using Dotnetools.Fluent.ViewModels.Navigation;

namespace Dotnetools.Fluent.ViewModels.Settings;

[NavigationMetaData(Title = "")]
public partial class ThemeChangeViewModel : RoutableViewModel
{
	private readonly Theme _newTheme;

	public ThemeChangeViewModel(Theme newTheme)
	{
		_newTheme = newTheme;
	}

	protected override void OnNavigatedTo(bool isInHistory, CompositeDisposable disposables)
	{
		base.OnNavigatedTo(isInHistory, disposables);

		RxApp.MainThreadScheduler.Schedule(async () =>
		{
			await Task.Delay(500);
			ThemeHelper.ApplyTheme(_newTheme);
			Navigate().Clear();
		});
	}
}
