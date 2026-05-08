using BetonBon.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.RegistrationMenu.Activities
{
    public partial class AllActivitiesView
    {
        private string Search = "";

        [Parameter, EditorRequired]
        public ProjectDTO SelectedProject { get; set; }

        [Parameter, EditorRequired]
        public bool IsVisible { get; set; }

        [Parameter, EditorRequired]
        public EventCallback OnClose { get; set; }

        [Parameter, EditorRequired]
        public EventCallback ProjectSelected { get; set; }


        private List<ActivityDTO> _activities = [];
        private IEnumerable<ActivityDTO> _filteredActivities = [];

        private async Task OnSearch()
        {
            _filteredActivities = string.IsNullOrWhiteSpace(Search)
                ? _activities
                : _activities.Where(p =>
                    p.Name.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                    p.Number.ToString().Contains(Search));
        }

        private async Task Close()
        {
            _filteredActivities = [];
            _activities = [];
            await OnClose.InvokeAsync();
        }

        private void SelectActivity()
        {

        }

        protected override async Task OnParametersSetAsync()
        {
            if (SelectedProject!= null && IsVisible == true) _activities = await _economicApi.GetAllActivitiesByProjectAsync(SelectedProject.Number);
            _filteredActivities = _activities;
        }

    }
}
