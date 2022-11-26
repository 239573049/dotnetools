using Newtonsoft.Json;
using Dotnetools.JsonConverters;

namespace Dotnetools.CoinJoin.Common.Models;

public class InputsResponse
{
	[JsonConverter(typeof(GuidJsonConverter))]
	public Guid UniqueId { get; set; }

	public long RoundId { get; set; }
}
