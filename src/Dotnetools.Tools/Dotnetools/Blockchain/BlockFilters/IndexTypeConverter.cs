using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dotnetools.BitcoinCore.Rpc.Models;

namespace Dotnetools.Blockchain.BlockFilters;

public static class IndexTypeConverter
{
	public static RpcPubkeyType[] ToRpcPubKeyTypes(IndexType indexType)
		=> indexType switch
		{
			IndexType.SegwitTaproot => new[] { RpcPubkeyType.TxWitnessV0Keyhash, RpcPubkeyType.TxWitnessV1Taproot },
			IndexType.Taproot => new[] { RpcPubkeyType.TxWitnessV1Taproot },
			_ => throw new NotSupportedException("Index type not supported."),
		};
}
