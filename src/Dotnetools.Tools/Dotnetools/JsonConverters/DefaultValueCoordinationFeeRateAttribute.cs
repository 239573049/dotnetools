using NBitcoin;
using System.ComponentModel;
using Dotnetools.WabiSabi.Models;

namespace Dotnetools.JsonConverters;

public class DefaultValueCoordinationFeeRateAttribute : DefaultValueAttribute
{
	public DefaultValueCoordinationFeeRateAttribute(double feeRate, double plebsDontPayThreshold)
		: base(new CoordinationFeeRate((decimal)feeRate, Money.Coins((decimal)plebsDontPayThreshold)))
	{
	}
}
