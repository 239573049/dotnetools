using Dotnetools.WabiSabi.Crypto.CredentialRequesting;

namespace Dotnetools.WabiSabi.Models;

public record ReissueCredentialResponse(
	CredentialsResponse RealAmountCredentials,
	CredentialsResponse RealVsizeCredentials,
	CredentialsResponse ZeroAmountCredentials,
	CredentialsResponse ZeroVsizeCredentials
);
