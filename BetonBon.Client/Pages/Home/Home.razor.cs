using BetonBon.Client.Auth;
using Microsoft.JSInterop;

namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {

        public bool IsDropdownVisible = false;
        public void NavigateToAdministration()
        {
            _navigation.NavigateTo("/administration");
        }

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
