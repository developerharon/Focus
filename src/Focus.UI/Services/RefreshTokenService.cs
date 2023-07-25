using Blazored.LocalStorage;
using Focus.Shared.DTOs;
using Focus.Shared.DTOs.Users;
using Focus.UI.Extensions;
using Focus.UI.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

namespace Focus.UI.Services
{
    public class RefreshTokenService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;
        private readonly LogoutService _logoutService;

        public RefreshTokenService(AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorageService, HttpClient httpClient, LogoutService logoutService)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
            _httpClient = httpClient;
            _logoutService = logoutService;
        }

        public async Task<string> TryRefreshTokenAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));

            var timeUTC = DateTime.UtcNow;

            var diff = expTime - timeUTC;
            if (diff.TotalMinutes <= 2)
                return await RefreshToken();
            return string.Empty;
        }

        private async Task<string> RefreshToken()
        {
            try
            {
                var refreshTokenDto = await _localStorageService.GetRefreshTokenDTOAsync();

                var response = await _httpClient.PostAsJsonAsync(ServerUrls.RefreshTokenUrl, refreshTokenDto);
                response.EnsureSuccessStatusCode();

                var responseDto = await JsonSerializer.DeserializeAsync<ResponseDTO<LoginResponseDTO>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                if (responseDto == null || responseDto.Data == null)
                    throw new Exception();

                await responseDto.Data.PersistLoginTokensAsync(_localStorageService);
                return responseDto.Data.AccessToken;
            }
            catch (Exception)
            {
                await _logoutService.LogoutAsync();
                throw new Exception("Error when refreshing the token");
            }
        }
    }
}
