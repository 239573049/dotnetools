using System.Collections.Generic;
using NBitcoin;
using Dotnetools.Blockchain.Keys;
using Dotnetools.Crypto;

namespace Dotnetools.WabiSabi.Client;

public interface IKeyChain
{
	OwnershipProof GetOwnershipProof(IDestination destination, CoinJoinInputCommitmentData committedData);

	Transaction Sign(Transaction transaction, Coin coin, OwnershipProof ownershipProof);

	void TrySetScriptStates(KeyState state, IEnumerable<Script> scripts);
}
