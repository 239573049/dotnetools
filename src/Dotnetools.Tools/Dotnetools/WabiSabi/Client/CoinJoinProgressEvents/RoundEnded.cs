using Dotnetools.WabiSabi.Models;

namespace Dotnetools.WabiSabi.Client.CoinJoinProgressEvents;

public class RoundEnded : CoinJoinProgressEventArgs
{
	public RoundEnded(RoundState lastRoundState)
	{
		LastRoundState = lastRoundState;
	}

	public RoundState LastRoundState { get; }
	public bool IsStopped { get; set; }
}
