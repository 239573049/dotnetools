using ReactiveUI;
using Dotnetools.Blockchain.Analysis.Clustering;
using Dotnetools.Fluent.ViewModels.Dialogs.Base;

namespace Dotnetools.Fluent.ViewModels.Dialogs;

public class ConfirmHideAddressViewModel : DialogViewModelBase<bool>
{
	private string _title;

	public ConfirmHideAddressViewModel(SmartLabel label)
	{
		Label = label;
		_title = "Hide Address";

		NextCommand = ReactiveCommand.Create(() => Close(result: true));
		CancelCommand = ReactiveCommand.Create(() => Close(DialogResultKind.Cancel));

		SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);
	}

	public SmartLabel Label { get; }

	public override string Title
	{
		get => _title;
		protected set => this.RaiseAndSetIfChanged(ref _title, value);
	}
}
