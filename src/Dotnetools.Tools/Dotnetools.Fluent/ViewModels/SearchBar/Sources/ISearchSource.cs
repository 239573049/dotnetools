using DynamicData;
using Dotnetools.Fluent.ViewModels.SearchBar.Patterns;
using Dotnetools.Fluent.ViewModels.SearchBar.SearchItems;

namespace Dotnetools.Fluent.ViewModels.SearchBar.Sources;

public interface ISearchSource
{
	IObservable<IChangeSet<ISearchItem, ComposedKey>> Changes { get; }
}
