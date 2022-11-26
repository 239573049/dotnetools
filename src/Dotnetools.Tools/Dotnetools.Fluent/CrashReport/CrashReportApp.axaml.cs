using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Dotnetools.Fluent.CrashReport.ViewModels;
using Dotnetools.Models;
using Dotnetools.Fluent.CrashReport.Views;

namespace Dotnetools.Fluent.CrashReport;

public class CrashReportApp : Application
{
	private readonly SerializableException? _serializableException;

	public CrashReportApp()
	{
		Name = "Wasabi Wallet Crash Report";
	}

	public CrashReportApp(SerializableException exception) : this()
	{
		_serializableException = exception;
	}

	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}

	public override void OnFrameworkInitializationCompleted()
	{
		if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && _serializableException is { })
		{
			desktop.MainWindow = new CrashReportWindow
			{
				DataContext = new CrashReportWindowViewModel(_serializableException)
			};
		}

		base.OnFrameworkInitializationCompleted();
	}
}
