using ClassifiedAds.CrossCuttingConcerns.ExtensionMethods;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClassifiedAds.Blazor.Modules.Core.Services
{
    public class HttpService
    {
        protected readonly HttpClient _httpClient;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenProvider _tokenProvider;

        public HttpService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, TokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _tokenProvider = tokenProvider;
        }

        public async Task<string> GetAccessToken()
        {
            //var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            //return accessToken;

            if(_tokenProvider.TokenExpired)
            {
                // TODO: refresh token
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
