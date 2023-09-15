using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Tests.IntegrationTests;

public class TranslationJobIntegrationTests : IntegrationTest
{
    public TranslationJobIntegrationTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Test()
    {
        var items = await _dbContext.TranslationJobs.Select(i => i).ToListAsync();
        Assert.True(true);
    }
}
