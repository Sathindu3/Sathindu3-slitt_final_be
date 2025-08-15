using sliit_final_be.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class RoboflowService
{
    private readonly string _apiKey;
    private readonly string _modelEndpoint;

    public RoboflowService(string apiKey, string modelEndpoint)
    {
        _apiKey = apiKey;
        _modelEndpoint = modelEndpoint;
    }

    public async Task<string> DetectAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File cannot be null or empty.", nameof(file));

        using var httpClient = new HttpClient();

        using var content = new MultipartFormDataContent();

        // Add the file
        using var fileStream = file.OpenReadStream();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
        content.Add(fileContent, "file", file.FileName);

        // Add API key as query parameter (or header, depending on Roboflow docs)
        var requestUrl = $"{_modelEndpoint}?api_key={_apiKey}";

        // Send POST request
        var response = await httpClient.PostAsync(requestUrl, content);

        response.EnsureSuccessStatusCode(); // Throws if status != 2xx

        var responseString = await response.Content.ReadAsStringAsync();
        return responseString;
    }
}
