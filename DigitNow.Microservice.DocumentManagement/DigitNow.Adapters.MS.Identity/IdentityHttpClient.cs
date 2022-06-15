using Newtonsoft.Json;

namespace DigitNow.Adapters.MS.Identity
{
    public class IdentityHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;

        public IdentityHttpClient(IServiceProvider serviceProvider, string identityApiAddress)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(identityApiAddress);
            _serviceProvider = serviceProvider;
        }

        public async Task<T> GetAsync<T>(string requestUri) where T : class
        {
            var response = await _httpClient.GetAsync(requestUri);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(responseContent);
            if (result == default(T))
            {
                throw new InvalidDataException();
            }

            return result;
        }
    }
}
