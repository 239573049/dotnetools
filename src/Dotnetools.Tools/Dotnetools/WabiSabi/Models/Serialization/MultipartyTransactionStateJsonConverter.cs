using Dotnetools.WabiSabi.Models.MultipartyTransaction;

namespace Dotnetools.WabiSabi.Models.Serialization;

public class MultipartyTransactionStateJsonConverter : GenericInterfaceJsonConverter<MultipartyTransactionState>
{
	public MultipartyTransactionStateJsonConverter() : base(new[] { typeof(ConstructionState), typeof(SigningState) })
	{
	}
}
