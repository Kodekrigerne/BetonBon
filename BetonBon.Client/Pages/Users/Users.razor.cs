using BetonBon.Client.Shared.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages.Users
{
    public partial class Users
    {
        private UserViewModel? selectedUser;
        private UpdateUserModel? editModel;

        private bool isCreating { get; set; } = false;
        private bool isEditing = false;

        [Parameter] public EventCallback OnCloseUsers { get; set; }


        private List<UserViewModel>? users;

        protected override async Task OnParametersSetAsync()
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
                await OnCloseUsers.InvokeAsync();
            }
        }



        private void HandleCreateUser()
        {
            isCreating = true;
        }

        private async Task HandleDeleteUser()
        {
            if (selectedUser == null) return;

            await _api.DeleteUser(selectedUser.Id);

            isEditing = false;
            selectedUser = null;

            await LoadUsers();
        }


        private async Task HandleUserCreated()
        {
            isCreating = false;
            isEditing = false;
            selectedUser = null;
            editModel = null;

            await LoadUsers();
        }

        private async Task HandleEditUser(Guid id)
        {
            selectedUser = users?.FirstOrDefault(u => u.Id == id);
            if (selectedUser != null)
            {
                editModel = new UpdateUserModel
                {
                    Id = selectedUser.Id,
                    Username = selectedUser.Name,
                    Role = selectedUser.Role
                };
                isEditing = true;
            }
        }
    }
}
