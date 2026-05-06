using BetonBon.Client.Pages.Projects;

namespace BetonBon.Client.Pages.Home
{
    public partial class Home
    {
        public void CloseProjects()
        {
            IsVisibleProjects = false;
        }

        public void Openprojects()
        {
            IsVisibleProjects = true;
        }

        public bool IsVisibleProjects = false;
    }
}
