using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;
    }
}
