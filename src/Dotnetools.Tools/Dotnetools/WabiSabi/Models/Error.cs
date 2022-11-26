using Dotnetools.WabiSabi.Backend.Models;

namespace Dotnetools.WabiSabi.Models;

public record Error(
	string Type,
	string ErrorCode,
	string Description,
	ExceptionData ExceptionData
);
