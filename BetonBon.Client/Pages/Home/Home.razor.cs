using BetonBon.Client.Pages.Activities;
using BetonBon.Client.Pages.Projects;
using BetonBon.Shared.Models;

namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {
        public bool IsVisibleProjects = false;
        public bool IsVisibleUsers = false;

        public void CloseProjects()
        {
            IsVisibleProjects = false;
            IsVisibleActivities = false;
        }

        public void OpenProjects()
        {
            IsVisibleProjects = true;
        }

        public void OpenUsers() => IsVisibleUsers = true;
        public void CloseUsers() => IsVisibleUsers = false;

        public void SelectProject(ProjectDTO projectDTO)
        {
            SelectedProject = projectDTO;
            IsVisibleActivities = true;
        }

        public void CloseActivities()
        {
            IsVisibleActivities = false;
            SelectedProject = null;
        }

        public ProjectDTO? SelectedProject;
        public bool IsVisibleProjects = false;
        public bool IsVisibleActivities = false;
    }
}
