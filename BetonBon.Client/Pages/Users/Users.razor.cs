using BetonBon.Client.Shared.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.Users
{
    public partial class Users
    {
        [Parameter] public bool UsersIsVisible { get; set; } = true;
        private bool isCreating { get; set; } = false;
        [Parameter] public EventCallback OnCloseUsers { get; set; }


        private List<UserViewModel>? users;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadUsers();

            isCreating = false;
        }

        private async Task LoadUsers()
        {
            var userList = await _api.GetAllUsers();
            users = userList.Select(u => new UserViewModel(u.Id, u.Username, u.Role)).ToList();
        }

        public async Task CloseUsers()
        {
            if (isCreating)
            {
                isCreating = false;
            }
            else
            { 
                UsersIsVisible = false; 
            
                await OnCloseUsers.InvokeAsync();
            }
        }



        private void HandleCreateUser()
        {
            isCreating = true;
        }

        private async Task HandleUserCreated()
        {
            isCreating = false;
            await LoadUsers();
        }

        private void EditUser(Guid id)
        {

        }
    }
}
