using External.ThirdParty.Services;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public class NotificationService
{
    private readonly UnreliableNotificationService notificationService;

    public NotificationService(UnreliableNotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    public async Task Notify(string text)
    {
        try
        {
            var result = await notificationService.SendNotification(text);

            if (!result)
                Failed();
        }
        catch (Exception)
        {
            Failed();
            throw;
        }
    }

    private void Failed()
    {
        // Handle SendNotification faild
    }
}
