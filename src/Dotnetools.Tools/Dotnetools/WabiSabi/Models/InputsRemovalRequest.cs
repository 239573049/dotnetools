using NBitcoin;

namespace Dotnetools.WabiSabi.Models;

public record InputsRemovalRequest(
	uint256 RoundId,
	Guid AliceId
);
