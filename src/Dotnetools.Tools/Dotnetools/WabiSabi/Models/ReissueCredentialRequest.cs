using NBitcoin;
using Dotnetools.WabiSabi.Crypto.CredentialRequesting;

namespace Dotnetools.WabiSabi.Models;

public record ReissueCredentialRequest(
	uint256 RoundId,
	RealCredentialsRequest RealAmountCredentialRequests,
	RealCredentialsRequest RealVsizeCredentialRequests,
	ZeroCredentialsRequest ZeroAmountCredentialRequests,
	ZeroCredentialsRequest ZeroVsizeCredentialsRequests
);
