using BetonBon.Shared.Models;
using BetonBon.Shared.Models.Activities;

namespace BetonBon.Client.Pages.RegistrationMenu
{
    public class RegistrationState
    {
        public ProjectDTO? SelectedProject { get; set; }
        public ActivityDTO? SelectedActivity { get; set; }
        public MaterialDTO? SelectedMaterial { get; set; }

        public void ClearState()
        {
            SelectedProject = null;
            SelectedActivity = null;
            SelectedMaterial = null;
        }
    }
}
