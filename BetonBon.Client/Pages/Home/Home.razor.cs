using BetonBon.Client.Pages.Projects;

namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {
        public bool IsVisibleProjects = false;
        public bool IsVisibleUsers = false;

        public void CloseProjects()
        {
            IsVisibleProjects = false;
        }

        public void Openprojects()
        {
            IsVisibleProjects = true;
        }

        public void OpenUsers() => IsVisibleUsers = true;
        public void CloseUsers() => IsVisibleUsers = false;

    }
}
