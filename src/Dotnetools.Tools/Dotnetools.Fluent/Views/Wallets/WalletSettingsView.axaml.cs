using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Dotnetools.Fluent.Views.Wallets;

public class WalletSettingsView : UserControl
{
	public WalletSettingsView()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}
