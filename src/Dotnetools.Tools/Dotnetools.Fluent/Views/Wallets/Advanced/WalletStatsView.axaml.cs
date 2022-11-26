using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Dotnetools.Fluent.Views.Wallets.Advanced;

public class WalletStatsView : UserControl
{
	public WalletStatsView()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}
}
