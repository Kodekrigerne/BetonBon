using BetonBon.Client.Pages.Activities;
using BetonBon.Client.Pages.Projects;
using BetonBon.Shared.Models;

namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {
        public void CloseProjects()
        {
            IsVisibleProjects = false;
            IsVisibleActivities = false;
        }

        public void OpenProjects()
        {
            IsVisibleProjects = true;
        }

        public void SelectProject(ProjectDTO projectDTO)
        {
            SelectedProject = projectDTO;
            IsVisibleActivities = true;
            IsVisibleProjects = false;
        }

        public void CloseActivities()
        {
            IsVisibleActivities = false;
            IsVisibleProjects = true;
            SelectedProject = null;
        }

        public ProjectDTO? SelectedProject;
        public bool IsVisibleProjects = false;
        public bool IsVisibleActivities = false;
    }
}
