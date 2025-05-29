namespace Laboratory8.Interfaces;

public interface INotificationService
{
    void ShowNotification(string title, string message, int notificationId = 0);
    void ScheduleNotification(string title, string message, TimeSpan delay, int notificationId = 0);
}