using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence.Repositories;
using Shared.Abstraction;
using Shared.Abstraction.Repositories;
using Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection(DatabaseSettings.Key));
        services.AddSingleton(s => s.GetRequiredService<IOptions<DatabaseSettings>>().Value);

        services.AddDbContext<ApplicationDbContext>((p, m) =>
        {
            var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            m.UseSqlite(databaseSettings.ConnectionString);
        });

        services.AddSingleton<ITranslationJobRepository, TranslationJobRepository>();
        services.AddSingleton<ITranslatorRepository, TranslatorRepository>();

        //services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        return services;
    }
}
