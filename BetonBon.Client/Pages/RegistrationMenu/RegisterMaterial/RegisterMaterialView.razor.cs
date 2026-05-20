using System.Globalization;
using BetonBon.Shared.Models;
using BetonBon.Shared.Models.Activities;
using BetonBon.Shared.Models.DraftEntries;
using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.RegistrationMenu.RegisterMaterial
{
    public partial class RegisterMaterialView
    {

        [Parameter, EditorRequired]
        public ProjectDTO Project { get; set; }

        [Parameter, EditorRequired]
        public ActivityDTO Activity { get; set; }

        [Parameter, EditorRequired]
        public bool ShowMaterialModal { get; set; } = false;

        [Parameter, EditorRequired]
        public EventCallback OnClosed { get; set; }

        [Parameter, EditorRequired]
        public EventCallback OnChangeActivity { get; set; }
        [Parameter, EditorRequired]
        public EventCallback OnChangeProject { get; set; }
        
        public int MyProperty { get; set; }

        private DateTime RegistrationDate { get; set; } = DateTime.Today;
        private string Note { get; set; } = "";

        private MaterialDTO? SelectedMaterial;

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
            SelectedMaterial = null;
            StateHasChanged();
        }

        private async Task SelectMaterial(MaterialDTO material)
        {
            SelectedMaterial = material;
            _materialsPickerIsVisible = false;
        }

        private async Task GoBack()
        {
            await OnClosed.InvokeAsync();
        }

        private void OpenPicker()
        {
            _materialsPickerIsVisible = true;
        }

        private async Task ChangeActivity()
        {
            SelectedMaterial = null;
            await OnChangeActivity.InvokeAsync();
        }

        private async Task ChangeProject()
        {
            SelectedMaterial = null;
            await OnChangeProject.InvokeAsync();
        }

        private void SaveButtonPressed()
        {
            if (SelectedMaterial != null && Amount > 0)
            {
                ConfirmationIsVisible = true;
            }
        }

        private async Task SaveRegistration()
        {
            if (SelectedMaterial != null && Amount > 0)
            {
                try
                {
                    string date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);

                    var timeEntry = new NewDraftEntryDTO()
                    {
                        Date = date,
                        Amount = Amount,
                        ProjectNumber = Project.Number,
                        CostTypeNumber = SelectedMaterial.Number,
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