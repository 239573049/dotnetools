using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Dotnetools.Fluent.Helpers;

public static class ApplicationHelper
{
	public static Window? MainWindow => (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
}
