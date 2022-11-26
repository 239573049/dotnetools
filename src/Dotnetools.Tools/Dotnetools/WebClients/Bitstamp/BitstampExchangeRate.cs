using Newtonsoft.Json;

namespace Dotnetools.WebClients.Bitstamp;

public class BitstampExchangeRate
{
	[JsonProperty(PropertyName = "bid")]
	public decimal Rate { get; set; }
}
