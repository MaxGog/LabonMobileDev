using Laboratory8.Interfaces;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace Laboratory8.Servives;

public class NotificationService : Interfaces.INotificationService
{
    public void ShowNotification(string title, string message, int notificationId = 0)
    {
        var request = new NotificationRequest
        {
            NotificationId = notificationId,
            Title = title,
            Description = message,
            ReturningData = "Dummy data", // Optional
            Android = new AndroidOptions
            {
                //IconName = "icon",
                AutoCancel = true,
                ChannelId = "general"
            }
        };

        LocalNotificationCenter.Current.Show(request);
    }

    public void ScheduleNotification(string title, string message, TimeSpan delay, int notificationId = 0)
    {
        var request = new NotificationRequest
        {
            NotificationId = notificationId,
            Title = title,
            Description = message,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.Add(delay)
            },
            Android = new AndroidOptions
            {
                //IconName = "icon",
                AutoCancel = true,
                ChannelId = "general"
            }
        };

        LocalNotificationCenter.Current.Show(request);
    }
}