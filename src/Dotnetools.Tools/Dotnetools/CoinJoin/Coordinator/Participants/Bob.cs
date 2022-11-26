using NBitcoin;
using Dotnetools.CoinJoin.Coordinator.MixingLevels;
using Dotnetools.Helpers;

namespace Dotnetools.CoinJoin.Coordinator.Participants;

public class Bob
{
	public Bob(BitcoinAddress activeOutputAddress, MixingLevel level)
	{
		ActiveOutputAddress = Guard.NotNull(nameof(activeOutputAddress), activeOutputAddress);
		Level = Guard.NotNull(nameof(level), level);
	}

	public MixingLevel Level { get; }
	public BitcoinAddress ActiveOutputAddress { get; }
}
