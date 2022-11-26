using Dotnetools.Blockchain.Keys;
using Dotnetools.Blockchain.TransactionProcessing;

namespace Dotnetools.Tests.Helpers;

public static class TransactionProcessorExtensions
{
	public static HdPubKey NewKey(this TransactionProcessor me, string label)
	{
		return me.KeyManager.GenerateNewKey(label, KeyState.Clean, true);
	}
}
