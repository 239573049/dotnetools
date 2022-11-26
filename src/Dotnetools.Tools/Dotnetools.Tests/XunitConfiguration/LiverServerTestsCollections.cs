using Xunit;

namespace Dotnetools.Tests.XunitConfiguration;

[CollectionDefinition("LiveServerTests collection")]
public class LiverServerTestsCollections : ICollectionFixture<LiveServerTestsFixture>
{
}
