using Dotnetools.WabiSabi.Crypto.CredentialRequesting;

namespace Dotnetools.WabiSabi.Models;

public record InputRegistrationResponse(
	Guid AliceId,
	CredentialsResponse AmountCredentials,
	CredentialsResponse VsizeCredentials,
	bool IsPayingZeroCoordinationFee
);
