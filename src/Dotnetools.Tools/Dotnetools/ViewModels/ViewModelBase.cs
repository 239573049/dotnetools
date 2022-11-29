using Prism.Mvvm;
using Prism.Regions;

namespace Dotnetools.ViewModels;

public class ViewModelBase : BindableBase, INavigationAware
{
    private string _title;

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public virtual bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return OnNavigatingTo(navigationContext);
    }

    public virtual void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }

    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {
    }

    public virtual bool OnNavigatingTo(NavigationContext navigationContext)
    {
        return true;
    }
}