﻿using ClassifiedAds.CrossCuttingConcerns.ExtensionMethods;
using ClassifiedAds.Infrastructure.Web.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClassifiedAds.Blazor.Modules.Core.Services
{
    public class HttpService
    {
        protected readonly HttpClient _httpClient;
        protected readonly TokenProvider _tokenProvider;
        protected readonly TokenManager _tokenManager;

        public HttpService(HttpClient httpClient, TokenProvider tokenProvider, TokenManager tokenManager)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
            _tokenManager = tokenManager;
        }

        public async Task<string> GetAccessToken()
        {
            if (_tokenProvider.TokenExpired)
            {
                var tokens = await _tokenManager.RefreshToken(_tokenProvider.RefreshToken);

                if (tokens == null)
                {
                    // TODO: relogin
                }

                _tokenProvider.AccessToken = tokens.AccessToken;
                _tokenProvider.RefreshToken = tokens.RefreshToken;
                _tokenProvider.ExpiresAt = tokens.ExpiresAt;
            }

            return await Task.FromResult(_tokenProvider.AccessToken);
        }

        protected async Task SetBearerToken()
        {
            var accessToken = await GetAccessToken();
            if (accessToken != null)
            {
                _httpClient.UseBearerToken(accessToken);
            }
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            await SetBearerToken();

            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAs<T>();
            return data;
        }

        protected async Task<T> PostAsync<T>(string url, object data = null)
        {
            await SetBearerToken();

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (data != null)
            {
                request.Content = data.AsJsonContent();
            }

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var createdObject = await response.Content.ReadAs<T>();
            return createdObject;
        }

        public async Task<T> PutAsync<T>(string url, object data)
        {
            await SetBearerToken();

            var request = new HttpRequestMessage(HttpMethod.Put, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = data.AsJsonContent();

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var updatedObject = await response.Content.ReadAs<T>();
            return updatedObject;
        }

        protected async Task DeleteAsync(string url)
        {
            await SetBearerToken();

            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
        }
    }
}
