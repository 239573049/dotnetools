using NBitcoin;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dotnetools.JsonConverters;

namespace Dotnetools.CoinJoin.Common.Models;

public class ConnectionConfirmationResponse
{
	[JsonProperty(ItemConverterType = typeof(Uint256JsonConverter))]
	public IEnumerable<uint256> BlindedOutputSignatures { get; set; }

	[Required]
	public RoundPhase CurrentPhase { get; set; }
}
