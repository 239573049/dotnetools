using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using Dotnetools.Helpers;
using Dotnetools.Logging;
using Dotnetools.Tor;

namespace Dotnetools.Tests.Helpers;

public static class Common
{
	static Common()
	{
		Logger.SetFilePath(Path.Combine(DataDir, "Logs.txt"));
		Logger.SetMinimumLevel(LogLevel.Info);
		Logger.SetModes(LogMode.Debug, LogMode.File);
	}

	public static EndPoint TorSocks5Endpoint => new IPEndPoint(IPAddress.Loopback, 37150);
	public static string TorDistributionFolder => Path.Combine(EnvironmentHelpers.GetFullBaseDirectory(), "TorDaemons");
	public static TorSettings TorSettings => new(DataDir, TorDistributionFolder, terminateOnExit: false);

	public static string DataDir => EnvironmentHelpers.GetDataDir(Path.Combine("Dotnetools", "Tests"));

	public static string GetWorkDir([CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
	{
		return Path.Combine(DataDir, EnvironmentHelpers.ExtractFileName(callerFilePath), callerMemberName);
	}

	public static IEnumerable<TResult> Repeat<TResult>(Func<TResult> action, int count)
	{
		for (int i = 0; i < count; i++)
		{
			yield return action();
		}
	}
}
