using System.ComponentModel;

namespace Dotnetools.JsonConverters.Timing;

public class DefaultValueTimeSpanAttribute : DefaultValueAttribute
{
	public DefaultValueTimeSpanAttribute(string json) : base(TimeSpanJsonConverter.Parse(json))
	{
	}
}
