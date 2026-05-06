using BetonBon.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.Projects
{
    public partial class AllProjectsView
    {

        [Parameter]
        public bool ProjectsIsVisible { get; set; }

        [Parameter]
        public EventCallback<ProjectDTO> SelectProject { get; set; }

        [Parameter]
        public EventCallback OnCloseProjects { get; set; }

        public ProjectDTO? SelectedProject;

        private string Search { get; set; } = "";

        private List<ProjectDTO> Projects = [];

        private IEnumerable<ProjectDTO> FilteredProjects =>
            string.IsNullOrWhiteSpace(Search)
                ? Projects
                : Projects.Where(p =>
                    p.Name.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                    p.Number.ToString().Contains(Search));


        private async Task ClickProject(ProjectDTO selectedProject) => await SelectProject.InvokeAsync(selectedProject);

        protected override async Task OnInitializedAsync()
        {
            Projects = await GetProjects();
        }

        private async Task<List<ProjectDTO>> GetProjects()
        {
            return await _economicApi.GetAllProjectsAsync();
        }

        public async Task CloseProjects() => await OnCloseProjects.InvokeAsync();
    }
}
