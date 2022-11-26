using Dotnetools.Backend.Models.Responses;

namespace Dotnetools.WabiSabi.Client;

public interface IWasabiBackendStatusProvider
{
	SynchronizeResponse? LastResponse { get; }
}
