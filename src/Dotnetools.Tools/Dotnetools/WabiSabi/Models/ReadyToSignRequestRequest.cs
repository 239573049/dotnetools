using NBitcoin;

namespace Dotnetools.WabiSabi.Models;

public record ReadyToSignRequestRequest(uint256 RoundId, Guid AliceId);
