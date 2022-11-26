using Avalonia.Data.Converters;

namespace Dotnetools.Fluent.Converters;

public static class IntConverter
{
	public static readonly IValueConverter ToOrdinalString =
		new FuncValueConverter<int, string>(x => $"{x}.");
}
