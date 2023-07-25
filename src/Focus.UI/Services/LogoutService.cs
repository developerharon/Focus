using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Focus.UI.Extensions;
using Focus.UI.Infrastructure;

namespace Focus.UI.Services
{
    public class LogoutService
    {
        private NavigationManager _navigationManager;
        private HttpClient _httpClient;
        private AuthenticationStateProvider _authenticationStateProvider;
        private ILocalStorageService _localStorageService;

        public LogoutService(NavigationManager navigationManager, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorageService)
        {
            _navigationManager = navigationManager;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorageService = localStorageService;
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _httpClient.PostAsync(ServerUrls.LogoutUrl, null);
                _httpClient.RemoveBearerAuthenticationHeader();
                await _localStorageService.RemoveLoginTokensAsync();
                ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
                _navigationManager.NavigateTo("/");
            }
            catch (Exception)
            {
                // TODO: 
            }
        }
    }
}