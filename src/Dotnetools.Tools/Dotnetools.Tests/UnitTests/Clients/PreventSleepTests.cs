using System.Threading.Tasks;
using Dotnetools.Helpers;
using Xunit;

namespace Dotnetools.Tests.UnitTests.Clients;

public class PreventSleepTests
{
	[Fact]
	public async Task ProlongSystemAwakeCanBeExecutedAsync()
	{
		await EnvironmentHelpers.ProlongSystemAwakeAsync();
	}
}
