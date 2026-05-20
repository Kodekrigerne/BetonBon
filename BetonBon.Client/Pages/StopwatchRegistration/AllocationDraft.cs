using BetonBon.Shared.Models;
using BetonBon.Shared.Models.Activities;

namespace BetonBon.Client.Pages.StopwatchRegistration
{
    public class AllocationDraft
    {
        public Guid Id { get; set; }
        public ProjectDTO? ProjectDTO { get; set; }
        public ActivityDTO? ActivityDTO { get; set; }
        public int Minutes { get; set; }
    }
}