using NBitcoin;
using Dotnetools.WabiSabi.Crypto.CredentialRequesting;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Dotnetools.WabiSabi.Models;

public record OutputRegistrationRequest(
	uint256 RoundId,
	[ValidateNever] Script Script,
	RealCredentialsRequest AmountCredentialRequests,
	RealCredentialsRequest VsizeCredentialRequests
);
