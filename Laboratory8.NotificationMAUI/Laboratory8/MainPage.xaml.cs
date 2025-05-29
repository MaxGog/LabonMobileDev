using Laboratory8.Interfaces;

namespace Laboratory8;

public partial class MainPage : ContentPage
{
    private readonly INotificationService _notificationService;
    private int _notificationId = 100;

    public MainPage(INotificationService notificationService)
    {
        InitializeComponent();
        _notificationService = notificationService;
    }

    private void OnImmediateNotificationClicked(object sender, EventArgs e)
    {
        _notificationService.ShowNotification("MAUI Уведомление", 
            "Это тестовое уведомление", _notificationId++);
    }

    private void OnScheduledNotificationClicked(object sender, EventArgs e)
    {
        _notificationService.ScheduleNotification("MAUI Напоминание", 
            "Это запланированное уведомление", TimeSpan.FromSeconds(10), _notificationId++);
        
        DisplayAlert("Info", "Уведомление запланировано на 10 секунд", "OK");
    }
}

