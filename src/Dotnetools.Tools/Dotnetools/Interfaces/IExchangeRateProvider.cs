using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dotnetools.Backend.Models;

namespace Dotnetools.Interfaces;

public interface IExchangeRateProvider
{
	Task<IEnumerable<ExchangeRate>> GetExchangeRateAsync(CancellationToken cancellationToken);
}
