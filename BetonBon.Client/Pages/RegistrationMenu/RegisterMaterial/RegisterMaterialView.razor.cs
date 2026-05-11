using System.Diagnostics;
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

        private int Quantity { get; set; } = 1;

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
            Quantity = 1;
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
        private void IncreaseQuantity()
        {
            Quantity++;
        }

        private void DecreaseQuantity()
        {
            if (Quantity > 1)
            {
                Quantity--;
            }
        }

        private void SaveRegistration()
        {
           // TODO: Save Registration logic
        }
    }
}