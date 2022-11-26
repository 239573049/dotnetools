using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Dotnetools.Fluent.Screenshot;

namespace Dotnetools.Fluent.Views;

public class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
#if DEBUG
		this.AttachDevTools();
		this.AttachCapture();
#endif
	}
}
