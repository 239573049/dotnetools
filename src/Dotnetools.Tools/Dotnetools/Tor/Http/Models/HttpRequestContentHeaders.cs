using System.Net.Http.Headers;

namespace Dotnetools.Tor.Http.Models;

public record HttpRequestContentHeaders(
	HttpRequestHeaders RequestHeaders,
	HttpContentHeaders ContentHeaders);
