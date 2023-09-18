using Core.Services;
using Domain.Entities;
using Domain.Enumerations;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstraction.Repositories;
using Shared.ApiModels;
using Shared.ApiModels.Dtos.TranslationJobs;
using Shared.ApiModels.Dtos.Translators;
using System.Net;

namespace Api.Tests.IntegrationTests;

public class TranslatorIntegrationTests : IntegrationTest
{
    private readonly ITranslatorRepository translatorRepository;
    private readonly TranslationJobService translationJobService;
    private readonly TranslatorService translatorService;

    public TranslatorIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
    {
        translatorRepository = scope.ServiceProvider.GetRequiredService<ITranslatorRepository>();
        translationJobService = scope.ServiceProvider.GetRequiredService<TranslationJobService>();
        translatorService = scope.ServiceProvider.GetRequiredService<TranslatorService>();
    }

    [Fact]
    public async Task Should_Return_Empty_Result()
    {
        var content = await httpClient.GetAsync<List<TranslatorDto>>("api/v1/Translator/list");
        Assert.Empty(content);
    }

    [Fact]
    public async Task Should_Return_NotFound_Result()
    {
        var response = await httpClient.GetAsync("api/v1/Translator/1");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Should_Return_Two_Items()
    {
        await translatorRepository.Create(new Translator
        {
            Name = "Translator 1",
            HourlyRate = 1,
            CreditCardNumber = "",
            Status = TranslatorStatus.Applicant
        });

        await translatorRepository.Create(new Translator
        {
            Name = "Translator 2",
            HourlyRate = 1,
            CreditCardNumber = "",
            Status = TranslatorStatus.Applicant
        });

        var content = await httpClient.GetAsync<List<TranslatorDto>>("api/v1/Translator/list");

        Assert.Equal(2, content.Count);
    }

    [Fact]
    public async Task Should_Add_Translator_And_Return_One_Item()
    {
        var translatorCommand = new CreateTranslatorCommand
        {
            Name = "Translator",
            HourlyRate = 1,
            CreditCardNumber = "",
        };

        var createResponse = await httpClient.PostAsync("api/v1/Translator", null, translatorCommand);

        Assert.True(createResponse.IsSuccessStatusCode);

        var list = await httpClient.GetAsync<List<TranslatorDto>>("api/v1/Translator/list");

        Assert.Single(list);
        Assert.Equal(translatorCommand.Name, list[0].Name);
    }

    [Fact]
    public async Task Should_Approve_Translator()
    {
        var translatorId = await translatorRepository.Create(new Translator
        {
            Name = "Translator 1",
            HourlyRate = 1,
            CreditCardNumber = "",
            Status = TranslatorStatus.Applicant
        });

        var createResponse = await httpClient.PutAsync($"api/v1/Translator/{translatorId}/approve");

        Assert.True(createResponse.IsSuccessStatusCode);

        var item = await httpClient.GetAsync<TranslatorDto>("api/v1/Translator/" + translatorId);

        Assert.Equal(TranslatorStatus.Certified, item.Status);
    }

    [Fact]
    public async Task Should_Assign_One_Job()
    {
        var jobId = await translationJobService.CreateJob("Customer 1", "Text 1");

        var translatorId = await translatorRepository.Create(new Translator
        {
            Name = "Translator 1",
            HourlyRate = 1,
            CreditCardNumber = "",
            Status = TranslatorStatus.Applicant
        });

        await translatorService.Approve(translatorId);

        var queryParams = new Dictionary<string, string>
        {
            { "jobId", $"{jobId}" }
        };

        var createResponse = await httpClient.PutAsync($"api/v1/Translator/{translatorId}/job", queryParams);

        Assert.True(createResponse.IsSuccessStatusCode);

        var item = await httpClient.GetAsync<TranslatorDetailDto>("api/v1/Translator/" + translatorId);

        Assert.Single(item.TranslationJobs);
    }
}
