using Dotnetools.WabiSabi.Backend.Models;

namespace Dotnetools.WabiSabi.Models.Serialization;

public class ExceptionDataJsonConverter : GenericInterfaceJsonConverter<ExceptionData>
{
	public ExceptionDataJsonConverter() : base(new[] { typeof(InputBannedExceptionData), typeof(EmptyExceptionData), typeof(WrongPhaseExceptionData) })
	{
	}
}
