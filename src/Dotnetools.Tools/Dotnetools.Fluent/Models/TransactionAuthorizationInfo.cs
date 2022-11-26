using NBitcoin;
using Dotnetools.Blockchain.TransactionBuilding;
using Dotnetools.Blockchain.Transactions;

namespace Dotnetools.Fluent.Models;

public class TransactionAuthorizationInfo
{
	public TransactionAuthorizationInfo(BuildTransactionResult buildTransactionResult)
	{
		Psbt = buildTransactionResult.Psbt;
		Transaction = buildTransactionResult.Transaction;
	}

	public SmartTransaction Transaction { get; set; }

	public PSBT Psbt { get; }
}
