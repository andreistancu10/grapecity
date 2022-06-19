using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

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
            await SetAuthorizationTokenAsync();

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

        private async Task SetAuthorizationTokenAsync()
        {
            var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
