using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Api.Tests;

public class IntegrationTest : IDisposable, IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient? _httpClient;
    private readonly IServiceScope _scope;
    protected readonly WebApplicationFactory<Program> _factory;
    //protected readonly IDependentService _dependentService;
    protected readonly ApplicationDbContext _dbContext;
    protected readonly IMapper _mapper;

    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("https://localhost:7124")
        });
        _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");

        _scope = factory.Services.CreateScope();
        //_dependentService = _scope.ServiceProvider.GetRequiredService<IDependentService>();
        _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
        _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _dbContext.Database.EnsureCreated();
    }

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                _httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                    BaseAddress = new Uri("https://localhost:7124")
                });
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }

    public void Dispose()
    {
        //_dbContext.Dependents.ExecuteDelete();
        //_dbContext.Employees.ExecuteDelete();
        _scope.Dispose();
        HttpClient.Dispose();
    }
}