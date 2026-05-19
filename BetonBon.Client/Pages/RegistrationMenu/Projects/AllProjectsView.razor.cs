using BetonBon.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.RegistrationMenu.Projects
{
    public partial class AllProjectsView
    {

        [Parameter, EditorRequired]
        public bool IsVisible { get; set; }

        [Parameter, EditorRequired]
        public EventCallback<ProjectDTO> SelectProject { get; set; }

        [Parameter, EditorRequired]
        public EventCallback OnCloseProjects { get; set; }

        public ProjectDTO? SelectedProject;

        private string Search { get; set; } = "";

        private List<ProjectDTO> _projects = [];
        private IEnumerable<ProjectDTO> _filteredProjects = [];

        private async Task OnSearch()
        {

            Console.WriteLine($"Search Fired!! Term: {Search}");
            _filteredProjects = string.IsNullOrWhiteSpace(Search)
                ? _projects
                : [.. _projects.Where(p =>
                    p.Name.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                    p.Number.ToString().Contains(Search))];

            StateHasChanged();
        }


        private async Task ClickProject(ProjectDTO selectedProject) => await SelectProject.InvokeAsync(selectedProject);

        protected override async Task OnInitializedAsync()
        {
            if (IsVisible == true && (_projects == null || _projects.Count == 0))
            {
                try
                {
                    _projects = await _economicApi.GetAllProjectsAsync();
                    _filteredProjects = _projects;
                }
                catch (Exception e)
                {
                    await _popUpService.AlertAsync($"Der var et problem med at hente projekter. Prøv at lukke af og på igen.\n\nFejlbesked: {e.Message}");
                }
            }
        }

        protected override async Task OnParametersSetAsync()
        {

            if (IsVisible == true && (_projects == null || _projects.Count == 0))
            {
                try
                {
                    _projects = await _economicApi.GetAllProjectsAsync();
                    _filteredProjects = _projects;
                }
                catch (Exception e)
                {
                    await _popUpService.AlertAsync($"Der var et problem med at hente projekter. Prøv at lukke af og på igen.\n\nFejlbesked: {e.Message}");
                }
            }        }

        private async Task CloseProjects() => await OnCloseProjects.InvokeAsync();
    }
}
