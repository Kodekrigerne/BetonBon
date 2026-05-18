using System.Text.Json;
using BetonBon.Client.Pages.Home;
using BetonBon.Client.RefitInterfaces;
using BetonBon.Client.Services;
using BetonBon.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BetonBon.Client.Pages.StopwatchRegistration
{
    public partial class StopwatchRegistration
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public IJSRuntime JS { get; set; } = null!;

        [Inject]
        public LocalStorage LocalStorage { get; set; } = null!;

        [Inject]
        public IEconomicRelayApi EconomicApi { get; set; } = null!;

        [Inject]
        public TimeEntryService TimeEntryService { get; set; } = null!;

        private bool _projectsIsVisible;
        private bool _activitiesIsVisible;
        private Guid? _selectingForDraftId;
        private ProjectDTO? _selectedProject;

        private bool _initialized;
        private StopwatchSession _session = null!;
        private readonly List<AllocationDraft> _allocationDrafts = [];
        private int _totalMinutes;

        private int TotalAllocated => _allocationDrafts.Sum(d => d.Minutes);
        private int RemainingMinutes => _totalMinutes - TotalAllocated;
        private bool AllAssigned => _allocationDrafts.All(d => d.ProjectDTO != null && d.ActivityDTO != null);

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var json = await LocalStorage.LoadAsync("bb_timer")
                    ?? throw new InvalidOperationException("Invalid state: Navigated to stopwatch-registration with no session recorded.");

                _session = JsonSerializer.Deserialize<StopwatchSession>(json)
                    ?? throw new InvalidOperationException("Invalid stopwatch session data.");

                _totalMinutes = Math.Max(1, (int)Math.Round(_session.GetOffset().TotalMinutes));

                _allocationDrafts.Add(new AllocationDraft
                {
                    Id = Guid.NewGuid(),
                    Minutes = _totalMinutes
                });

                _initialized = true;
                StateHasChanged();
            }
        }

        private async Task NavigateBack()
        {
            await JS.InvokeVoidAsync("history.back");
        }

        private void RemoveAllocation(Guid id)
        {
            _allocationDrafts.RemoveAll(d => d.Id == id);
        }

        private void UpdateMinutes(Guid id, int delta)
        {
            var draft = _allocationDrafts.FirstOrDefault(d => d.Id == id);
            if (draft == null) return;

            var maxAddable = RemainingMinutes;
            var actualDelta = delta;

            if (delta > 0 && delta > maxAddable)
                actualDelta = maxAddable;

            if (draft.Minutes + actualDelta < 0)
                actualDelta = -draft.Minutes;

            draft.Minutes += actualDelta;
        }

        private void AddAllocation()
        {
            if (RemainingMinutes == 0) return;

            var newId = Guid.NewGuid();
            _allocationDrafts.Add(new AllocationDraft { Id = newId });
            SelectProjectForAllocation(newId);
        }

        private async Task SaveAllocations()
        {
            if (_allocationDrafts.Any(d => d.ProjectDTO == null || d.ActivityDTO == null)) return; //TODO: Popup, snack bar?

            var responses = await TimeEntryService.CreateTimeEntries(_session, _allocationDrafts);
            //TODO: Handle fails

            NavigationManager.NavigateTo("/");
        }

        private string FormatSessionElapsed()
        {
            var ts = _session.GetOffset();
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours}t {ts.Minutes}m {ts.Seconds}s";
            if (ts.TotalMinutes >= 1)
                return $"{ts.Minutes}m {ts.Seconds}s";
            return $"{ts.Seconds}s";
        }

        private static string FormatMinutes(int totalMinutes)
        {
            if (totalMinutes == 0) return "0 min";
            var h = totalMinutes / 60;
            var m = totalMinutes % 60;
            if (h > 0) return $"{h}t {m}m";
            return $"{m} min";
        }

        private void SelectProjectForAllocation(Guid draftId)
        {
            _selectingForDraftId = draftId;
            _projectsIsVisible = true;
        }

        private void HandleProjectSelected(ProjectDTO project)
        {
            _selectedProject = project;
            _projectsIsVisible = false;
            _activitiesIsVisible = true;
        }

        private void HandleProjectsClosed()
        {
            _projectsIsVisible = false;
        }

        private void HandleActivitySelected(ActivityDTO activity)
        {
            var draft = _allocationDrafts.Single(x => x.Id == _selectingForDraftId);
            draft.ProjectDTO = _selectedProject;
            draft.ActivityDTO = activity;
            _activitiesIsVisible = false;
        }

        private void HandleActivitiesClosed()
        {
            _activitiesIsVisible = false;
            _selectedProject = null;
            _selectingForDraftId = null;
        }
    }
}
