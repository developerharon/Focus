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

        public static async Task RemoveLoginTokensAsync(this ILocalStorageService localStorageService)
        {
            await localStorageService.RemoveItemAsync("accessToken");
            await localStorageService.RemoveItemAsync("refreshToken");
        }

        public static async Task<RefreshTokenDTO> GetRefreshTokenDTOAsync(this ILocalStorageService localStorageService)
        {
            var accessToken  = await localStorageService.GetItemAsync<string>("authToken");
            var refreshToken = await localStorageService.GetItemAsync<string>("refreshToken");

            return new RefreshTokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}