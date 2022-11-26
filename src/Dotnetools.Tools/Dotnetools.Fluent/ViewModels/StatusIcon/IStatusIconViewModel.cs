using System.Collections.Generic;
using System.Windows.Input;
using Dotnetools.Tor.StatusChecker;

namespace Dotnetools.Fluent.ViewModels.StatusIcon;

public interface IStatusIconViewModel
{
	ICollection<Issue> TorIssues { get; }
	ICommand OpenTorStatusSiteCommand { get; }
}
