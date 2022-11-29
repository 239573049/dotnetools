using Avalonia;
using Avalonia.Markup.Xaml;
using Dotnetools.Services;
using Dotnetools.ViewModels;
using Dotnetools.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Regions;

namespace Dotnetools;

public class App : PrismApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();
    }

    protected override void OnInitialized()
    {
        var regionManager = Container.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(DashboardView));
        regionManager.RegisterViewWithRegion(RegionNames.SidebarRegion, typeof(SidebarView));
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<INotifictionService, NotifictionService>();

        containerRegistry.Register<SidebarView>();
        containerRegistry.Register<MainWindow>();

        containerRegistry.RegisterForNavigation<DashboardView, DashboardViewModel>();
        containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
        containerRegistry.RegisterForNavigation<SubSettingsView, SubSettingsViewModel>();
    }

    protected override IAvaloniaObject CreateShell()
    {
        return Container.Resolve<MainView>();
    }
}