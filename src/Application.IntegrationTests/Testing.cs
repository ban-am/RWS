using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Respawn;

namespace Application.IntegrationTests;

[SetUpFixture]
public partial class Testing
{
    private static WebApplicationFactory<Program> factory;
    private static IConfiguration configuration;
    private static IServiceScopeFactory scopeFactory;
    private static Respawner checkpoint;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        factory = new CustomWebApplicationFactory();
        scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        configuration = factory.Services.GetRequiredService<IConfiguration>();


        checkpoint = Respawner.CreateAsync(GetConnectionString(), new RespawnerOptions
        {
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        }).GetAwaiter().GetResult();
    }

    private static string GetConnectionString()
    {
        return configuration.GetConnectionString("ConnectionString");
    }

    public static async Task ResetState()
    {
        try
        {
            await checkpoint.ResetAsync(GetConnectionString());
        }
        catch (Exception)
        {
        }
    }

    public static HttpClient CreateHttpClient()
    {
        return factory.CreateClient();
    }
}