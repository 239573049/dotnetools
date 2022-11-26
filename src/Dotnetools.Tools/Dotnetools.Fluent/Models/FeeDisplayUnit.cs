using Dotnetools.Models;

namespace Dotnetools.Fluent.Models;

public enum FeeDisplayUnit
{
	BTC,

	[FriendlyName("sats")]
	Satoshis,
}
