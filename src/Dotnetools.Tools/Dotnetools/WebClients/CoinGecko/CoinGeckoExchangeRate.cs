using Newtonsoft.Json;

namespace Dotnetools.WebClients.CoinGecko;

public class CoinGeckoExchangeRate
{
	[JsonProperty(PropertyName = "current_price")]
	public decimal Rate { get; set; }
}
