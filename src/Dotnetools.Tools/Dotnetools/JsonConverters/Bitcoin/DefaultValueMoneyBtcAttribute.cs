using System.ComponentModel;

namespace Dotnetools.JsonConverters.Bitcoin;

public class DefaultValueMoneyBtcAttribute : DefaultValueAttribute
{
	public DefaultValueMoneyBtcAttribute(string json) : base(MoneyBtcJsonConverter.Parse(json))
	{
	}
}
