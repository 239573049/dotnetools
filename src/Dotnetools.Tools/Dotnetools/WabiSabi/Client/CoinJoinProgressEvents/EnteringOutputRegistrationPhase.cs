using Dotnetools.WabiSabi.Models;

namespace Dotnetools.WabiSabi.Client.CoinJoinProgressEvents;

public class EnteringOutputRegistrationPhase : RoundStateChanged
{
	public EnteringOutputRegistrationPhase(RoundState roundState, DateTimeOffset timeoutAt) : base(roundState, timeoutAt)
	{
	}
}
