using ReactiveUI;
using Dotnetools.Fluent.Models;

namespace Dotnetools.Fluent.ViewModels.AddWallet;

public partial class AddWalletPageOption : ReactiveObject
{
	[AutoNotify] private string? _title;
	[AutoNotify] private string? _iconName;

	public WalletCreationOption CreationOption { get; init; }
}
