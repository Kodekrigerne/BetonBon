
namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {

        public void NavigateToAdministration()
        {
            _navigation.NavigateTo("/administration");
        }

        public void OpenRegistrations()
        {
            _navigation.NavigateTo("/registration");
        }
    }
}
