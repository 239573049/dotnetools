using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnetools.Wallets;

namespace Dotnetools.WabiSabi.Client;

public interface IWalletProvider
{
	Task<IEnumerable<IWallet>> GetWalletsAsync();
}
