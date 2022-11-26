using Dotnetools.WabiSabi.Backend.Rounds;

namespace Dotnetools.WabiSabi.Backend.Models;

public record WrongPhaseExceptionData(Phase CurrentPhase) : ExceptionData;
