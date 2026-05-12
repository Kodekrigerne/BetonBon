
namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {
        public void NavigateToUsers()
        {
            _navigation.NavigateTo("/users");
        }

        public void OpenRegistrations()
        {
            _navigation.NavigateTo("/registration");
        }
    }
}
