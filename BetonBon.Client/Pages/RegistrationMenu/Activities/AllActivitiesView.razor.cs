using BetonBon.Shared.Models;
using BetonBon.Shared.Models.Activities;
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
        public EventCallback<ActivityDTO> OnActivitySelected { get; set; }


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
            await OnClose.InvokeAsync();
        }

        private async Task SelectActivity(ActivityDTO activity)
        {
            await OnActivitySelected.InvokeAsync(activity);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (SelectedProject != null && IsVisible == true && (_activities == null || _activities.Count == 0))
            {
                try
                {
                    _activities = await _economicApi.GetAllActivitiesByProjectAsync(SelectedProject.Number);
                    _filteredActivities = _activities;
                }
                catch (Exception ex)
                {
                    await _popUpService.AlertAsync($"Der var et problem med at hente aktiviteter. Prøv at lukke af og på igen.\n\nFejlbesked: {ex.Message}");
                }
            }
        }

    }
}
