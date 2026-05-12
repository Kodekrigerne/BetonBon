using BetonBon.Client.Auth;
using Microsoft.JSInterop;

namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {
        public bool IsVisibleUsers = false;

        public bool IsDropdownVisible = false;

        public void OpenUsers() => IsVisibleUsers = true;
        public void CloseUsers() => IsVisibleUsers = false;

        public void OpenRegistrations()
        {
            _navigation.NavigateTo("/registration");
        }

        public void ToggleDropdown()
        {
            IsDropdownVisible = !IsDropdownVisible;
        }

        public async Task Logout()
        {
            await jsRuntime.InvokeVoidAsync("storage.remove", "bb_token");
            await jsRuntime.InvokeVoidAsync("storage.remove", "bb_refresh_token");

            var customAuthStateProvider = (CustomAuthStateProvider)AuthStateProvider;

            await customAuthStateProvider.UpdateAuthenticationStatus();
        }
    }
}
