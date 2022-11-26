using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;
using Dotnetools.Blockchain.Keys;
using Dotnetools.Fluent.ViewModels.AddWallet;
using Dotnetools.Fluent.ViewModels.Dialogs;
using Dotnetools.Fluent.ViewModels.Dialogs.Base;

namespace Dotnetools.Fluent.ViewModels.CoinJoinProfiles;

[NavigationMetaData(Title = "Coinjoin Strategy")]
public partial class CoinJoinProfilesViewModel : DialogViewModelBase<bool>
{
	[AutoNotify] private CoinJoinProfileViewModelBase? _selectedProfile;

	public CoinJoinProfilesViewModel(KeyManager keyManager, bool isNewWallet)
	{
		NextCommand = ReactiveCommand.Create(() => OnNext(keyManager, isNewWallet));
		EnableBack = true;

		Profiles = DefaultProfiles.ToList();

		ManualSetupCommand = ReactiveCommand.CreateFromTask(async () => await OnManualSetupAsync());

		if (isNewWallet)
		{
			_selectedProfile = Profiles[1];
			return;
		}

		_selectedProfile = IdentifySelectedProfile(keyManager);
	}

	private static CoinJoinProfileViewModelBase[] DefaultProfiles { get; } = new CoinJoinProfileViewModelBase[]
	{
			new EconomicCoinJoinProfileViewModel(),
			new SpeedyCoinJoinProfileViewModel(),
			new PrivateCoinJoinProfileViewModel()
	};

	public static CoinJoinProfileViewModelBase IdentifySelectedProfile(KeyManager keyManager)
	{
		var currentProfile = new ManualCoinJoinProfileViewModel(keyManager);
		var result = DefaultProfiles.FirstOrDefault(x => x == currentProfile) ?? currentProfile;

		return result;
	}

	public ICommand ManualSetupCommand { get; }

	public List<CoinJoinProfileViewModelBase> Profiles { get; }

	public ManualCoinJoinProfileViewModel? SelectedManualProfile { get; private set; }

	private async Task OnManualSetupAsync()
	{
		var current = SelectedProfile ?? SelectedManualProfile ?? Profiles.First();
		var dialog = new ManualCoinJoinProfileDialogViewModel(current);

		var dialogResult = await NavigateDialogAsync(dialog, NavigationTarget.CompactDialogScreen);

		if (dialogResult.Result is ManualCoinJoinProfileDialogViewModel.ManualCoinJoinProfileDialogViewModelResult result)
		{
			SelectedProfile = null;
			SelectedManualProfile = result.Profile;
		}
	}

	private void OnNext(KeyManager keyManager, bool isNewWallet)
	{
		var selected = SelectedProfile ?? SelectedManualProfile ?? Profiles.First();

		keyManager.RedCoinIsolation = selected.RedCoinIsolation;
		keyManager.SetAnonScoreTarget(selected.AnonScoreTarget, toFile: false);
		keyManager.SetFeeRateMedianTimeFrame(selected.FeeRateMedianTimeFrameHours, toFile: false);
		keyManager.IsCoinjoinProfileSelected = true;

		if (isNewWallet)
		{
			Navigate().To(new AddedWalletPageViewModel(keyManager));
		}
		else
		{
			keyManager.ToFile();
			Close(DialogResultKind.Normal, true);
		}
	}

	protected override void OnNavigatedTo(bool isInHistory, CompositeDisposable disposables)
	{
		base.OnNavigatedTo(isInHistory, disposables);

		var enableCancel = Services.WalletManager.HasWallet();
		SetupCancel(enableCancel: false, enableCancelOnEscape: enableCancel, enableCancelOnPressed: false);
	}
}