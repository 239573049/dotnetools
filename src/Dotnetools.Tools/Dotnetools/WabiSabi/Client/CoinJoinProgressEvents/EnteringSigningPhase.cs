using Dotnetools.WabiSabi.Models;

namespace Dotnetools.WabiSabi.Client.CoinJoinProgressEvents;

public class EnteringSigningPhase : RoundStateChanged
{
	public EnteringSigningPhase(RoundState roundState, DateTimeOffset timeoutAt) : base(roundState, timeoutAt)
	{
	}
}
