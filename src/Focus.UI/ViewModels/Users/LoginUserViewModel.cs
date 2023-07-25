using Blazored.LocalStorage;
using Focus.Shared.DTOs;
using Focus.Shared.DTOs.Users;
using Focus.UI.Extensions;
using Focus.UI.Infrastructure;
using Focus.UI.ViewModels.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

namespace Focus.UI.ViewModels.Users
{
    public class LoginUserViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private LoginRequestDTO _loginRequestDTO = new LoginRequestDTO();

        public LoginRequestDTO LoginRequestDTO
        {
            get => _loginRequestDTO;
            set => SetValue(ref _loginRequestDTO, value);
        }

        public LoginUserViewModel(HttpClient httpClient, NavigationManager navigationManager, ILocalStorageService localStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task LoginUserAsync()
        {
            IsBusy = true;
            HttpResponseMessage? response = null;
            try
            {
                response = await _httpClient.PostAsJsonAsync(ServerUrls.LoginUrl, LoginRequestDTO);
                response.EnsureSuccessStatusCode();

                var responseDto = await JsonSerializer.DeserializeAsync<ResponseDTO<LoginResponseDTO>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (responseDto == null || responseDto.Data == null)
                    throw new Exception();

                await responseDto.Data.PersistLoginTokensAsync(_localStorageService);
                _httpClient.SetBearerAuthenticationHeader(responseDto.Data.AccessToken);
                ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserAuthentication(responseDto.Data.AccessToken);

                _navigationManager.NavigateTo("/");
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest && response != null)
                {
                    var responseDto = await JsonSerializer.DeserializeAsync<ResponseDTO<LoginResponseDTO>>(await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    Message = responseDto?.Message ?? "An error occurred";
                }
                else Message = ex.Message;
            }
            catch (Exception)
            {
                Message = "An error occurred. Try again.";
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}