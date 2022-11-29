using Avalonia.Controls;
using System;

namespace Dotnetools.Services;

public interface INotifictionService
{
    int NotificationTimeout { get; set; }

    void SetHostWindow(Window window);

    void Show(string title, string message, Action? onClick = null);
}