using BetonBon.Client.Pages.Activities;
using BetonBon.Client.Pages.Projects;
using BetonBon.Shared.Models;

namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {
        public bool IsVisibleUsers = false;

        public void OpenUsers() => IsVisibleUsers = true;
        public void CloseUsers() => IsVisibleUsers = false;

        public void OpenRegistrations()
        {
            _navigation.NavigateTo("/registration");
        }
    }
}
