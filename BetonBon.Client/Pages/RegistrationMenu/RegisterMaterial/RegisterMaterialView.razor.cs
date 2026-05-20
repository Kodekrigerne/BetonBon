using System.Globalization;
using BetonBon.Shared.Models;
using BetonBon.Shared.Models.Activities;
using BetonBon.Shared.Models.DraftEntries;
using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.RegistrationMenu.RegisterMaterial
{
    public partial class RegisterMaterialView
    {        
        private DateTime RegistrationDate { get; set; } = DateTime.Today;
        private string Note { get; set; } = "";

        private double Amount { get; set; }

        private bool ConfirmationIsVisible = false;
        private bool _materialsPickerIsVisible = false;

        private bool IsToday()
        {
            return RegistrationDate == DateTime.Today;
        }

        private async Task CloseMaterialPicker()
        {
            _materialsPickerIsVisible = false;
            State.SelectedMaterial = null;
            StateHasChanged();
        }

        private async Task SelectMaterial(MaterialDTO material)
        {
            State.SelectedMaterial = material;
            _materialsPickerIsVisible = false;
        }

        private async Task GoBack()
        {
            Navigation.NavigateTo("/registration/menu");
        }

        private void OpenPicker()
        {
            _materialsPickerIsVisible = true;
        }

        private async Task ChangeActivity()
        {
            State.SelectedMaterial = null;
            Navigation.NavigateTo("registration/activities");

        }

        private async Task ChangeProject()
        {
            State.SelectedMaterial = null;
            Navigation.NavigateTo("/registration/projects");
        }

        private void SaveButtonPressed()
        {
            if (State.SelectedMaterial != null && Amount > 0)
            {
                ConfirmationIsVisible = true;
            }
        }

        private async Task SaveRegistration()
        {
            if (State.SelectedProject!= null && State.SelectedActivity!= null && State.SelectedMaterial != null && Amount > 0)
            {
                try
                {
                    string date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);

                    var timeEntry = new NewDraftEntryDTO()
                    {
                        Date = date,
                        Amount = Amount,
                        ProjectNumber = State.SelectedProject.Number,
                        CostTypeNumber = State.SelectedMaterial.Number,
                        Text = Note
                    };

                    var result = await _economicApi.CreateNewDraftEntry(timeEntry);

                    if (result.IsSuccessStatusCode)
                    {
                        ReturnToHome();
                    }
                    else
                    {
                        await _popUp.AlertAsync($"Der var et problem med at oprette materialeregistreringen. Prøv at logge af og på igen. \n\nFejl: {result.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    await _popUp.AlertAsync($"Der var et problem med at oprette materialeregistreringen. Prøv at lukke af og på igen.\n\nFejlbesked: {ex.Message}");
                }
            }
        }

        private void CancelConfirm()
        {
            ConfirmationIsVisible = false;
        }

        private void ReturnToHome()
        {
            Navigation.NavigateTo("/");
        }
    }
}