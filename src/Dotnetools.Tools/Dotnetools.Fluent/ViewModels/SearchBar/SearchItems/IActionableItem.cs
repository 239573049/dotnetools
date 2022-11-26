using System.Threading.Tasks;

namespace Dotnetools.Fluent.ViewModels.SearchBar.SearchItems;

public interface IActionableItem : ISearchItem
{
	Func<Task> OnExecution { get; }
}
