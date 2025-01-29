using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class MyFunction
{
    private readonly ILogger<MyFunction> _logger;
    private readonly HttpClient _httpClient;

    public MyFunction(ILogger<MyFunction> logger)
    {
        _logger = logger;
        _httpClient = new HttpClient();
    }

    [Function("GetExternalData")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        _logger.LogInformation("Function triggered - Fetching external data...");

        // Extract 'url' from the query parameters
        var queryParams = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        string url = queryParams["url"];

        if (string.IsNullOrEmpty(url))
        {
            _logger.LogError("No URL provided in the request.");
            var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badRequestResponse.WriteStringAsync("Error: No 'url' parameter provided.");
            return badRequestResponse;
        }

        _logger.LogInformation($"Fetching data from: {url}");

        try
        {
            // Make HTTP GET request
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            string content = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Successfully fetched response.");
            
            // Create a response to return content
            var functionResponse = req.CreateResponse(HttpStatusCode.OK);
            functionResponse.Headers.Add("Content-Type", "text/plain");
            await functionResponse.WriteStringAsync(content);

            return functionResponse;
        }

        catch (Exception ex)
        {
            _logger.LogError($"Error fetching data: {ex.Message}");

            var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync($"Error: {ex.Message}");
            return errorResponse;
        }
    }
}
