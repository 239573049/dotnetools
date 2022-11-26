using NBitcoin;

namespace Dotnetools.WabiSabi.Models;

public record CoinJoinFeeRateMedian(TimeSpan TimeFrame, FeeRate MedianFeeRate);
