using System.Diagnostics.CodeAnalysis;
using NBitcoin;

namespace Dotnetools.Blockchain.Transactions;

public interface ITransactionStore
{
	public bool TryGetTransaction(uint256 hash, [NotNullWhen(true)] out SmartTransaction? sameStx);
}
