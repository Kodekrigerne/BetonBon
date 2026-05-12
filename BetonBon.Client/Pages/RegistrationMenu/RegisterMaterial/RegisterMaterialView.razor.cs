using System.Diagnostics;
using System.Globalization;
using BetonBon.Shared.Models;
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

        private DateTime RegistrationDate { get; set; } = DateTime.Today;
        private string Note { get; set; } = "";

        private MaterialDTO? SelectedMaterial;

        private double EnteredPrice { get; set; } = 5;

        private int Amount { get; set; } = 0;

        private bool _materialsPickerIsVisible = false;

        // Computed property for at tjekke om datoen er "i dag"
        private bool IsToday => RegistrationDate.Date == DateTime.Today;

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
            Amount = 0;
        }

        private async Task GoBack()
        {
            await OnClosed.InvokeAsync();
        }

        private void OpenPicker()
        {
            _materialsPickerIsVisible = true;
        }

        private void ChangeActivity()
        {
            // TODO: ChangeActiity in material registration?
        }

        private async Task SaveRegistration()
        {
            if (SelectedMaterial != null && Amount >= 1)
            {
                string date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);

                var result = await _economicApi.CreateNewDraftEntry(new NewDraftEntryDTO(date, Amount, Project.Number, SelectedMaterial.Id));


                if (result.IsSuccessStatusCode)
                {
                    ReturnToHome();
                }
            }
        }

        private void ReturnToHome()
        {
            Navigation.NavigateTo("/");
        }
    }
}