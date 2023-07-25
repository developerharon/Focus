using Blazored.LocalStorage;
using Focus.Shared.DTOs.Users;

namespace Focus.UI.Extensions
{
    public static class LoginResponseExtensions
    {
        public static async Task PersistLoginTokensAsync(this LoginResponseDTO? loginResponse, ILocalStorageService localStorageService)
        {
            if (loginResponse == null)
                return;

            await localStorageService.SetItemAsync("accessToken", loginResponse.AccessToken);
            await localStorageService.SetItemAsync("refreshToken", loginResponse.RefreshToken);
        }
    }
}