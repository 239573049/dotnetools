using System.Linq;
using Dotnetools.Helpers;
using static Dotnetools.Tor.Http.Constants;

namespace Dotnetools.Tor.Http.Models;

public abstract class StartLine
{
	protected StartLine(HttpProtocol protocol)
	{
		Protocol = protocol;
	}

	public HttpProtocol Protocol { get; }

	public static string[] GetParts(string startLineString)
	{
		var trimmed = Guard.NotNullOrEmptyOrWhitespace(nameof(startLineString), startLineString, trim: true);
		return trimmed.Split(SP, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
	}
}
