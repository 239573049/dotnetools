using System.Collections.Generic;
using Newtonsoft.Json;
using Dotnetools.JsonConverters;
using Dotnetools.JsonConverters.Bitcoin;
using Dotnetools.JsonConverters.Timing;
using Dotnetools.WabiSabi.Crypto.Serialization;

namespace Dotnetools.WabiSabi.Models.Serialization;

public class JsonSerializationOptions
{
	private static readonly JsonSerializerSettings CurrentSettings = new()
	{
		Converters = new List<JsonConverter>()
			{
				new ScalarJsonConverter(),
				new GroupElementJsonConverter(),
				new OutPointJsonConverter(),
				new WitScriptJsonConverter(),
				new ScriptJsonConverter(),
				new OwnershipProofJsonConverter(),
				new NetworkJsonConverter(),
				new FeeRateJsonConverter(),
				new MoneySatoshiJsonConverter(),
				new Uint256JsonConverter(),
				new MultipartyTransactionStateJsonConverter(),
				new ExceptionDataJsonConverter(),
				new ExtPubKeyJsonConverter(),
				new TimeSpanJsonConverter(),
				new CoinJsonConverter(),
				new CoinJoinEventJsonConverter(),
			}
	};
	public static readonly JsonSerializationOptions Default = new();

	private JsonSerializationOptions()
	{
	}

	public JsonSerializerSettings Settings => CurrentSettings;
}
