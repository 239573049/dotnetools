using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Dotnetools.Fluent.Views.Wallets;

public class ClosedWalletView : UserControl
{
	public ClosedWalletView()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}
