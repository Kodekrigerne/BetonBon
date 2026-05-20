using BetonBon.Shared.Models;
using BetonBon.Shared.Models.TimeEntries;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BetonBon.Client.Pages.RegistrationMenu.TimeEntries
{
    public partial class NewTimeEntryView
    {
        [Parameter, EditorRequired]
        public ProjectDTO Project { get; set; } = default!;

        [Parameter, EditorRequired]
        public ActivityDTO Activity { get; set; } = default!;

        [Parameter, EditorRequired]
        public bool IsVisible { get; set; }

        [Parameter, EditorRequired]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public EventCallback OnTimeEntryCreated { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> AuthStateTask { get; set; } = null!;

        private DateTime selectedDate = DateTime.Today;
        private int hours;
        private int minutes;
        private string? description;

        private bool isSubmitting;
        private bool showValidation;

        private double TotalHours => hours + (minutes / 60.0);

        private bool IsFormValid()
        {
            return hours > 0 || minutes > 0;
        }

        private void OnDateChanged(ChangeEventArgs e)
        {
            if (DateTime.TryParse(e.Value?.ToString(), out var date))
                selectedDate = date;
        }

        private void OnHoursChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var h))
                hours = Math.Clamp(h, 0, 24);
        }

        private void OnMinutesChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var m))
                minutes = Math.Clamp(m, 0, 59);
        }

        private string FormatTimeConfirmation()
        {
            if (hours > 0 && minutes > 0)
                return $"{hours} timer og {minutes} minutter";
            if (hours > 0)
                return $"{hours} timer";
            return $"{minutes} minutter";
        }

        private async Task HandleSubmit()
        {
            showValidation = true;

            if (!IsFormValid()) return;

            bool confirmed = await PopupService.ConfirmAsync(
                $"Registrer {FormatTimeConfirmation()} den {selectedDate:dd/MM/yyyy}?");

            if (!confirmed) return;

            isSubmitting = true;
            try
            {
                var authState = await AuthStateTask;

                var employeeNumberString = authState.User.FindFirst("employee_number")?.Value;
                if (employeeNumberString == null || !int.TryParse(employeeNumberString, out int employeeNumber))
                {
                    await PopupService.AlertAsync("Ugyldigt medarbejdernummer.");
                    isSubmitting = false;
                    return;
                }

                var timeEntry = new TimeEntry
                {
                    ProjectNumber = Project.Number,
                    ActivityNumber = Activity.Number,
                    EmployeeNumber = employeeNumber,
                    Date = new DateTimeOffset(DateTime.SpecifyKind(selectedDate, DateTimeKind.Utc), TimeSpan.Zero),
                    NumberOfHours = TotalHours,
                    Text = string.IsNullOrWhiteSpace(description) ? null : description.Trim()
                };

                var response = await EconomicApi.CreateTimeEntryAsync(timeEntry);

                if (response.IsSuccessStatusCode)
                {
                    await PopupService.AlertAsync("Tidsregistrering gemt!");
                    ResetForm();
                    await OnTimeEntryCreated.InvokeAsync();
                }
                else
                {
                    await PopupService.AlertAsync("Kunne ikke gemme tidsregistrering. Prøv igen.");
                }
            }
            catch (Exception)
            {
                await PopupService.AlertAsync("Der opstod en fejl. Prøv igen.");
            }
            finally
            {
                isSubmitting = false;
            }
        }

        private void ResetForm()
        {
            hours = 0;
            minutes = 0;
            description = null;
            showValidation = false;
            selectedDate = DateTime.Today;
        }

        private async Task OnClosed()
        {
            ResetForm();
            await OnClose.InvokeAsync();
        }
    }
}
