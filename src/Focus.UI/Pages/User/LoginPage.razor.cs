using Focus.Shared.DTOs.Users;
using Focus.UI.ViewModels.Users;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Focus.UI.Pages.User
{
    public partial class LoginPage : ComponentBase
    {
        [Inject] private LoginUserViewModel LoginUserViewModel { get; set; } = default!;

        private bool passwordVisibility;
        private InputType passwordInputType = InputType.Password;
        private string passwordInputIcon = Icons.Material.Filled.VisibilityOff;


        private void TogglePasswordVisibility()
        {
            if (passwordVisibility)
            {
                passwordVisibility = false;
                passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                passwordInputType = InputType.Password;
            }
            else
            {
                passwordVisibility = true;
                passwordInputIcon = Icons.Material.Filled.Visibility;
                passwordInputType = InputType.Text;
            }
        }
    }
}