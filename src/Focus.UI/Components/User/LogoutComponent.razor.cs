using Focus.UI.Extensions;
using Focus.UI.Services;
using Microsoft.AspNetCore.Components;

namespace Focus.UI.Components.User
{
    public partial class LogoutComponent : ComponentBase
    {
        [Inject] private LogoutService _logoutService { get; set; } = default!;

        private async Task LogoutAsync()
        {
            await _logoutService.LogoutAsync();
        }
    }
}