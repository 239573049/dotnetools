using Newtonsoft.Json;
using System.Collections.Generic;
using Dotnetools.JsonConverters;

namespace Dotnetools.Backend.Models.Responses;

public class FiltersResponse
{
	public int BestHeight { get; set; }

	[JsonProperty(ItemConverterType = typeof(FilterModelJsonConverter))] // Do not use the default jsonifyer, because that's too much data.
	public IEnumerable<FilterModel> Filters { get; set; } = new List<FilterModel>();
}
