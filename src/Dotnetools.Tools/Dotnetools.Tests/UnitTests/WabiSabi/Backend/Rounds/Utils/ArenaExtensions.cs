using System.Linq;
using System.Collections.Generic;
using Dotnetools.WabiSabi.Backend.Rounds;

namespace Dotnetools.Tests.UnitTests.WabiSabi.Backend.Rounds.Utils;

public static class ArenaExtensions
{
	public static IEnumerable<Round> GetActiveRounds(this Arena arena)
		=> arena.Rounds.Where(x => x.Phase != Phase.Ended);
}
