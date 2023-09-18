using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Api.Tests;

public abstract class IntegrationTest : IDisposable, IClassFixture<CustomWebApplicationFactory>
{
    protected HttpClient httpClient;
    protected static IServiceScopeFactory scopeFactory;
    protected readonly IServiceScope scope;
    protected readonly CustomWebApplicationFactory factory;
    protected readonly ApplicationDbContext dbContext;
    protected readonly IMapper mapper;

    public IntegrationTest(CustomWebApplicationFactory factory)
    {
        this.factory = factory;
        httpClient = factory.CreateClient();
        scope = factory.Services.CreateScope();
        scopeFactory = scope.ServiceProvider.GetService<IServiceScopeFactory>();
        dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }

    public void Dispose()
    {
        dbContext.TranslationJobs.ExecuteDelete();
        dbContext.Translators.ExecuteDelete();
        scope.Dispose();
        httpClient.Dispose();
    }
}
