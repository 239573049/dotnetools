using NBitcoin;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Dotnetools.JsonConverters;

namespace Dotnetools.CoinJoin.Common.Models;

public class InputProofModel
{
	[Required]
	[JsonConverter(typeof(OutPointAsTxoRefJsonConverter))]
	public OutPoint Input { get; set; }

	[Required]
	[JsonConverter(typeof(CompactSignatureJsonConverter))]
	public CompactSignature Proof { get; set; }
}
