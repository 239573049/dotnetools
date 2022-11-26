using System.Net;
using Dotnetools.Tor.Http.Helpers;
using Xunit;

namespace Dotnetools.Tests.UnitTests.Tor.Http.Helpers;

/// <summary>
/// Tests for <see cref="HttpStatusCodeHelper"/>.
/// </summary>
public class HttpStatusCodeHelperTests
{
	[Fact]
	public void CodeVerificationTest()
	{
		Assert.True(HttpStatusCodeHelper.IsInformational(HttpStatusCode.Processing));
		Assert.False(HttpStatusCodeHelper.IsInformational(HttpStatusCode.OK));

		Assert.True(HttpStatusCodeHelper.IsSuccessful(HttpStatusCode.OK));
		Assert.False(HttpStatusCodeHelper.IsSuccessful(HttpStatusCode.Redirect));
	}
}
