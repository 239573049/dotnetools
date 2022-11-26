using NBitcoin;
using System.Collections.Generic;

namespace Dotnetools.WabiSabi.Client;

public interface IDestinationProvider
{
	IEnumerable<IDestination> GetNextDestinations(int count);
}
