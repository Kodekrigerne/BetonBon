using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.Home
{
    public partial class UsersCard
    {
        [Parameter] public EventCallback OpenUsers { get; set; }

    }
}
