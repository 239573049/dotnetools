using Dotnetools.Helpers;
using Xunit;

namespace Dotnetools.Tests.UnitTests;

public class LoggerTests
{
	[Theory]
	[InlineData("./Program.cs")]
	[InlineData("\\Program.cs")]
	[InlineData("Program.cs")]
	[InlineData("C:\\User\\user\\Github\\Dotnetools\\Dotnetools.Fluent.Desktop\\Program.cs")]
	[InlineData("/mnt/C/User/user/Github/Dotnetools/Dotnetools.Fluent.Desktop/Program.cs")]
	[InlineData("~/Github/Dotnetools/Dotnetools.Fluent.Desktop/Program.cs")]
	[InlineData("Program")]
	public void EndPointParserTests(string path)
	{
		var sourceFileName = EnvironmentHelpers.ExtractFileName(path);
		Assert.Equal("Program", sourceFileName);
	}
}