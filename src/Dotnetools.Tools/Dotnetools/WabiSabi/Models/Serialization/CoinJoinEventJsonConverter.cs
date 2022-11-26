using Dotnetools.WabiSabi.Models.MultipartyTransaction;

namespace Dotnetools.WabiSabi.Models.Serialization;

public class CoinJoinEventJsonConverter : GenericInterfaceJsonConverter<IEvent>
{
	public CoinJoinEventJsonConverter() : base(new[] { typeof(InputAdded), typeof(OutputAdded), typeof(RoundCreated) })
	{
	}
}
