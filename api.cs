
using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnvironmentVariable; // Reference to the class holding environment variables
using System.Text.Json;

namespace ApiFunctions 
{


    //<summary>
    // Function to search for books via API
    //</summary>
    public class AnnaArchiveApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> SearchBookAsync(
            string searchTerm,
            string? author = null,
            int? limit = 10,
            string? lang = "en",
            string? category = null,
            string? fileType = null,
            string? sorting = "mostRelevant"
        )
        {
            var baseUrl = EnvironmentVariables.baseUrl;
            var apiKey = EnvironmentVariables.apiKey;
            var apiHost = EnvironmentVariables.apiHost;

            // Build the query string with optional parameters
            var query = $"?q={Uri.EscapeDataString(searchTerm)}&skip=0&limit={limit}&sort={Uri.EscapeDataString(sorting)}";

            if (!string.IsNullOrEmpty(author))
            {
                query += $"&author={Uri.EscapeDataString(author)}";
            }

            if (!string.IsNullOrEmpty(lang))
            {
                query += $"&lang={Uri.EscapeDataString(lang)}";
            }

            if (!string.IsNullOrEmpty(category))
            {
                query += $"&cat={Uri.EscapeDataString(category)}";
            }

            if (!string.IsNullOrEmpty(fileType))
            {
                query += $"&ext={Uri.EscapeDataString(fileType)}";
            }

            var requestUrl = $"{baseUrl}/search{query}";

            // Create the HttpRequestMessage
            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("x-rapidapi-host", apiHost);
            request.Headers.Add("x-rapidapi-key", apiKey);

            // Send the request and get the response
            HttpResponseMessage response = await client.SendAsync(request);

            // Ensure successful response
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
            else
            {
                throw new Exception($"Request failed with status code: {response.StatusCode}");
            }
        }
    }

    
}
