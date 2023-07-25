using Microsoft.AspNetCore.Components;

namespace Focus.UI.Components.User
{
    public partial class RedirectToLoginComponent : ComponentBase
    {
        [Inject] private NavigationManager _navigationManager { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            _navigationManager.NavigateTo("/user/login");
            await base.OnInitializedAsync();
        }
    }
}