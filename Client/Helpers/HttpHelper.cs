using System.Text.Json;

namespace Client.Helpers
{
    public interface IHttpHelper
    {
        public Task<T> GetAsync<T>(string url, CancellationToken cancellation = default);
    }

    public class HttpHelper : IHttpHelper
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly HttpClient _httpClient = new();

        public HttpHelper() {
            _httpClient.BaseAddress = new Uri("https://localhost:7237/");
        }

        

        public async Task<T> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            var res = await _httpClient.GetAsync(url, cancellationToken); 
            string json = await res.Content.ReadAsStringAsync();

            T? result = JsonSerializer.Deserialize<T>(json, _options) ?? default;

            return result!;
        } 

    }
}
