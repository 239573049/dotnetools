using NBitcoin;
using Dotnetools.Crypto;
using Dotnetools.WabiSabi.Crypto.CredentialRequesting;

namespace Dotnetools.WabiSabi.Models;

public record InputRegistrationRequest(
	uint256 RoundId,
	OutPoint Input,
	OwnershipProof OwnershipProof,
	ZeroCredentialsRequest ZeroAmountCredentialRequests,
	ZeroCredentialsRequest ZeroVsizeCredentialRequests
);
