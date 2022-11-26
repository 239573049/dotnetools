using Dotnetools.WabiSabi.Crypto.CredentialRequesting;

namespace Dotnetools.WabiSabi.Models;

public record ConnectionConfirmationResponse(
	CredentialsResponse ZeroAmountCredentials,
	CredentialsResponse ZeroVsizeCredentials,
	CredentialsResponse? RealAmountCredentials = null,
	CredentialsResponse? RealVsizeCredentials = null
);
