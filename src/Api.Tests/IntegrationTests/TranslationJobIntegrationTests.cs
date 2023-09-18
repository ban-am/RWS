using Core.Services;
using Domain.Enumerations;
using Microsoft.Extensions.DependencyInjection;
using Shared.ApiModels.Dtos.TranslationJobs;
using System.Net;

namespace Api.Tests.IntegrationTests;

public class TranslationJobIntegrationTests : IntegrationTest
{
    private readonly TranslationJobService translationJobService;

    public TranslationJobIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
        translationJobService = scope.ServiceProvider.GetRequiredService<TranslationJobService>();
    }

    [Fact]
    public async Task Should_Return_Empty_Result()
    {
        var content = await httpClient.GetAsync<List<TranslationJobDto>>("api/v1/TranslationJob/list");
        Assert.Empty(content);
    }

    [Fact]
    public async Task Should_Return_NotFound_Result()
    {
        var response = await httpClient.GetAsync("api/v1/TranslationJob/1");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Should_Return_Two_Items()
    {
        await translationJobService.CreateJob("Customer 1", "Text 1");
        await translationJobService.CreateJob("Customer 2", "Text 2");

        var content = await httpClient.GetAsync<List<TranslationJobDto>>("api/v1/TranslationJob/list");

        Assert.Equal(2, content.Count);
    }

    [Fact]
    public async Task Should_Add_TranslationJob_And_Return_One_Item()
    {
        var queryParams = new Dictionary<string, string>
        {
            { "customerName", "Customer 1" },
            { "contentToTranslate", "Text 1" }
        };

        var createResponse = await httpClient.PostAsync("api/v1/TranslationJob", queryParams);

        Assert.True(createResponse.IsSuccessStatusCode);

        var list = await httpClient.GetAsync<List<TranslationJobDto>>("api/v1/TranslationJob/list");

        Assert.Single(list);
    }

    [Fact]
    public async Task Should_Update_TranslationJob_Status()
    {
        var jobId = await translationJobService.CreateJob("Customer 1", "Text 1");

        var queryParams = new Dictionary<string, string>
        {
            { "status", $"{JobStatus.InProgress}" }
        };

        var createResponse = await httpClient.PutAsync($"api/v1/TranslationJob/{jobId}/status", queryParams);

        Assert.True(createResponse.IsSuccessStatusCode);

        var item = await httpClient.GetAsync<TranslationJobDetailDto>("api/v1/TranslationJob/" + jobId);

        Assert.Equal(JobStatus.InProgress, item.Status);
    }
}
