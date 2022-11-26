using NBitcoin;
using Dotnetools.Crypto;
using Dotnetools.Helpers;

namespace Dotnetools.CoinJoin.Common.Models;

public class ActiveOutput
{
	public ActiveOutput(BitcoinAddress address, UnblindedSignature signature, int mixingLevel)
	{
		Address = Guard.NotNull(nameof(address), address);
		Signature = Guard.NotNull(nameof(signature), signature);
		MixingLevel = Guard.MinimumAndNotNull(nameof(mixingLevel), mixingLevel, 0);
	}

	public BitcoinAddress Address { get; }
	public UnblindedSignature Signature { get; }
	public int MixingLevel { get; }
}