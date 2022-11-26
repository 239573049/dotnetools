using System.Linq;

namespace Dotnetools.WabiSabi.Client.CredentialDependencies;

public class ReissuanceNode : RequestNode
{
	public ReissuanceNode() :
		base(
			Enumerable.Repeat(0L, DependencyGraph.CredentialTypes.Count()),
			DependencyGraph.K,
			DependencyGraph.K,
			DependencyGraph.K * (DependencyGraph.K - 1))
	{
	}
}
