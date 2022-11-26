using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Dotnetools.Fluent.Views.Shell;

public class Shell : UserControl
{
	public Shell()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}
