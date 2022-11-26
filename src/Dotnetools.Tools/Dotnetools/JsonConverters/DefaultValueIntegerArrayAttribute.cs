using System.ComponentModel;

namespace Dotnetools.JsonConverters;

public class DefaultValueIntegerArrayAttribute : DefaultValueAttribute
{
	public DefaultValueIntegerArrayAttribute(string json) : base(IntegerArrayJsonConverter.Parse(json))
	{
	}
}
