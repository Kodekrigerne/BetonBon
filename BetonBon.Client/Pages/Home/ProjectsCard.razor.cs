using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.Home
{
    public partial class ProjectsCard
    {
        [Parameter]
        public EventCallback OpenProjects { get; set; } = default!;
    }
}
