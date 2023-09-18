using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace Api.Tests;

public static class HttpClientExtensions
{
    public static async Task<T> GetAsync<T>(this HttpClient httpClient, string url)
    {
        var response = await httpClient.GetAsync(url);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string url, Dictionary<string, string> queryParams = null, object body = null)
    {
        if (queryParams is not null && queryParams.Count > 0)
            url += new QueryBuilder(queryParams);

        StringContent reqeustBody = null;

        if (body is not null)
            reqeustBody = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        var postResponse = await httpClient.PostAsync(url, reqeustBody);
        return postResponse;
    }

    public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string url, Dictionary<string, string> queryParams = null, object body = null)
    {
        if (queryParams is not null && queryParams.Count > 0)
            url += new QueryBuilder(queryParams);

        StringContent reqeustBody = null;

        if (body is not null)
            reqeustBody = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        var postResponse = await httpClient.PutAsync(url, reqeustBody);
        return postResponse;
    }

}