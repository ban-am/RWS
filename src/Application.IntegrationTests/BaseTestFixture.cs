using NUnit.Framework;
using static Application.IntegrationTests.Testing;

namespace Application.IntegrationTests;

[TestFixture]
public abstract class BaseTestFixture
{
    protected HttpClient httpClient;

    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
        httpClient = CreateHttpClient();
    }
}
