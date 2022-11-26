using System.Collections.Generic;
using Dotnetools.Fluent.ViewModels.SearchBar.Patterns;

namespace Dotnetools.Fluent.ViewModels.SearchBar.SearchItems;

public interface ISearchItem
{
	string Name { get; }
	string Description { get; }
	ComposedKey Key { get; }
	string? Icon { get; set; }
	string Category { get; }
	IEnumerable<string> Keywords { get; }
	bool IsDefault { get; }
}