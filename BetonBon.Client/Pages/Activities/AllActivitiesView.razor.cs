using BetonBon.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.Activities
{
    public partial class AllActivitiesView
    {
        private string Search = "";


        [Parameter, EditorRequired]
        public ProjectDTO SelectedProject { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public EventCallback ProjectSelected { get; set; }

        private List<ActivityDTO> Activities = [];

        private IEnumerable<ActivityDTO> FilteredActivities =>
            string.IsNullOrWhiteSpace(Search)
                ? Activities
                : Activities.Where(p =>
                    p.Name.Contains(Search, StringComparison.OrdinalIgnoreCase) ||
                    p.Number.ToString().Contains(Search));


        private async Task Close()
        {
            Activities = [];
            await OnClose.InvokeAsync();
        }

        private void SelectActivity()
        {

        }

        protected override async Task OnParametersSetAsync()
        {
            if (SelectedProject != null) Activities = await _economicApi.GetAllActivitiesByProjectAsync(SelectedProject.Number);
        }

    }
}
