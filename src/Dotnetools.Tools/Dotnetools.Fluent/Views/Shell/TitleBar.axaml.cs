using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Dotnetools.Fluent.Views.Shell;

public class TitleBar : UserControl
{
	public TitleBar()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}
