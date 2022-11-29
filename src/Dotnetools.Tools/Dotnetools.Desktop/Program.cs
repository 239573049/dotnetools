using Avalonia;
using Avalonia.Logging;
using System;

namespace Dotnetools.Desktop;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        var builder = AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .With(new X11PlatformOptions { EnableMultiTouch = true, UseDBusMenu = true })
            .With(new Win32PlatformOptions { AllowEglInitialization = true })
            .UseSkia();
        //.LogToTrace()
        //.UseReactiveUI();
#if DEBUG
        builder.LogToTrace(LogEventLevel.Debug, LogArea.Property, LogArea.Layout, LogArea.Binding);
#endif
        return builder;
    }
}