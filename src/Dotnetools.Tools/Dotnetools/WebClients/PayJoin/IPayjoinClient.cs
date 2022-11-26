using System.Threading;
using System.Threading.Tasks;
using NBitcoin;
using Dotnetools.Blockchain.Keys;

namespace Dotnetools.WebClients.PayJoin;

public interface IPayjoinClient
{
	Uri PaymentUrl { get; }

	Task<PSBT> RequestPayjoin(PSBT originalTx, IHDKey accountKey, RootedKeyPath rootedKeyPath, HdPubKey changeHdPubKey, CancellationToken cancellationToken);
}
