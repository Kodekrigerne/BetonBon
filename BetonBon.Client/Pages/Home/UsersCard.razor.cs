using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.Home
{
    public partial class UsersCard
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        private void NavigateToUsers()
        {
            Navigation.NavigateTo("/users");
        }
    }
}
