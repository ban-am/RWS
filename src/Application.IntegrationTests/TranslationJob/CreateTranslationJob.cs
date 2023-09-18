using NUnit.Framework;
using Shared.ApiModels.Dtos.TranslationJobs;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests.TranslationJob;

public class CreateTranslationJob : BaseTestFixture
{
    //[Test]
    //public async Task Should_Add_Translation_Job()
    //{
    //    var response = await httpClient.GetAsync("api/v1/TranslationJob/list");
    //    var content = await response.Content.ReadFromJsonAsync<List<TranslationJobDto>>();
    //    content.Should().BeEmpty();
    //}

}
