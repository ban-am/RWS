using Core.Handlers;
using Core.Services;
using External.ThirdParty.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<NotificationService>();
        services.AddScoped<TranslatorService>();
        services.AddScoped<TranslationJobService>();
        services.AddScoped<TranslationJobPriceCalculatorService>();
        services.AddScoped<UnreliableNotificationService>();

        services.AddSingleton<FileHandlerFactory>();

        return services;
    }
}
