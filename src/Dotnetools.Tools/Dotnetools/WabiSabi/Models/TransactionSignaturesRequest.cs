using NBitcoin;

namespace Dotnetools.WabiSabi.Models;

public record TransactionSignaturesRequest(uint256 RoundId, uint InputIndex, WitScript Witness);
