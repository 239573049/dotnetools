using System.Reactive.Linq;
using ReactiveUI;
using Dotnetools.Blockchain.Analysis.Clustering;
using Dotnetools.Blockchain.Keys;
using Dotnetools.Fluent.ViewModels.Navigation;
using Dotnetools.Fluent.ViewModels.Wallets.Labels;

namespace Dotnetools.Fluent.ViewModels.Wallets.Receive;

[NavigationMetaData(Title = "Edit Labels")]
public partial class AddressLabelEditViewModel : RoutableViewModel
{
	[AutoNotify] private bool _isCurrentTextValid;

	public AddressLabelEditViewModel(ReceiveAddressesViewModel owner, HdPubKey hdPubKey, KeyManager keyManager)
	{
		SuggestionLabels = new SuggestionLabelsViewModel(keyManager, Intent.Receive, 3, hdPubKey.Label);

		SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);

		var canExecute =
			this.WhenAnyValue(x => x.SuggestionLabels.Labels.Count, x => x.IsCurrentTextValid)
				.Select(tup =>
				{
					var (labelsCount, isCurrentTextValid) = tup;
					return labelsCount > 0 || isCurrentTextValid;
				});

		NextCommand = ReactiveCommand.Create(
			() =>
			{
				hdPubKey.SetLabel(new SmartLabel(SuggestionLabels.Labels), kmToFile: keyManager);
				owner.InitializeAddresses();
				Navigate().Back();
			},
			canExecute);
	}

	public SuggestionLabelsViewModel SuggestionLabels { get; }
}
