using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.Home
{
    public partial class ProjectsCard
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        private void NavigateToProjects()
        {
            // Navigation.NavigateTo("/projects");
        }
    }
}
