using Avalonia.Controls;
using System;
using Avalonia;
using Avalonia.Controls.Notifications;

namespace Dotnetools.Services;

public class NotifictionService : INotifictionService
{
    private int _notificationTimeout = 10;
    private WindowNotificationManager? _notificationManager;

    public int NotificationTimeout
    {
        get => _notificationTimeout;
        set => _notificationTimeout = value < 0 ? 0 : value;
    }

    public void SetHostWindow(Window window)
    {
        var notificationManager = new WindowNotificationManager(window)
        {
            Position = NotificationPosition.BottomRight,
            MaxItems = 4,
            Margin = new Thickness(0, 0, 15, 40)
        };
        _notificationManager = notificationManager;
    }

    public void Show(string title, string message, Action? onClick = null)
    {
        if (_notificationManager is { } nm)
        {
            nm.Show(new Notification(title, message, NotificationType.Information,
                TimeSpan.FromSeconds(_notificationTimeout), onClick));
        }
    }
}