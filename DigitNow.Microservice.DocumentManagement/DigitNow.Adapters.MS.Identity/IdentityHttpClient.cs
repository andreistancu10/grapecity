﻿using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Text;
using System.Net;

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

        public async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken) where T : class
        {
            await SetAuthorizationTokenAsync();

            var response = await _httpClient.GetAsync(requestUri, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                throw new HttpRequestException(); //TODO: Throw a descriptive error
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(responseContent);
            if (result == default(T))
            {
                throw new InvalidDataException();
            }

            return result;
        }

        public async Task PostAsync(string requestUri, object content, CancellationToken cancellationToken)
        {
            await SetAuthorizationTokenAsync();

            var jsonObject = JsonConvert.SerializeObject(content);
            var response = await _httpClient.PostAsync(requestUri, new StringContent(jsonObject, Encoding.UTF8, "application/json"), cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                throw new HttpRequestException(); //TODO: Throw a descriptive error
            }
        }

        private async Task SetAuthorizationTokenAsync()
        {
            var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
