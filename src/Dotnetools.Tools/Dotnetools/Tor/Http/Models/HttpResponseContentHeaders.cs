using System.Net.Http.Headers;

namespace Dotnetools.Tor.Http.Models;

public record HttpResponseContentHeaders(
	HttpResponseHeaders ResponseHeaders,
	HttpContentHeaders ContentHeaders);
