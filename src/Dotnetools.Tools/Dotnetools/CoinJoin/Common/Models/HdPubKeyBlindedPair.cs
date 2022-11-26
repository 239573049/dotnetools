using Newtonsoft.Json;
using Dotnetools.Blockchain.Keys;
using Dotnetools.Helpers;

namespace Dotnetools.CoinJoin.Common.Models;

[JsonObject(MemberSerialization.OptIn)]
public class HdPubKeyBlindedPair
{
	[JsonConstructor]
	public HdPubKeyBlindedPair(HdPubKey key, bool isBlinded)
	{
		Key = Guard.NotNull(nameof(key), key);
		IsBlinded = isBlinded;
	}

	[JsonProperty]
	public HdPubKey Key { get; set; }

	[JsonProperty]
	public bool IsBlinded { get; set; }
}
