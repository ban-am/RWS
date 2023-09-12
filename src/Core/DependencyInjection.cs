using Core.Handlers;
using Core.Services;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Repositories;
using Shared.Abstraction.Repositories;
using Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<TranslatorService>();
        services.AddSingleton<TranslationJobService>();
        services.AddSingleton<TranslationJobPriceService>();
        services.AddSingleton<UnreliableNotificationService>();

        services.AddSingleton<FileHandlerFactory>();

        return services;
    }
}
