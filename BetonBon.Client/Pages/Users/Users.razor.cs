using BetonBon.Client.Shared.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BetonBon.Client.Pages.Users
{
    public partial class Users
    {
        private UserViewModel? selectedUser;
        private UpdateUserModel? editModel;
        private List<UserViewModel>? users;

        private bool isCreating { get; set; } = false;
        private bool isEditing = false;

        [Parameter] public EventCallback OnCloseUsers { get; set; }

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;


        protected override async Task OnParametersSetAsync()
        {
            await base.OnInitializedAsync();
            await LoadUsers();

            isCreating = false;
        }

        private async Task LoadUsers()
        {
            var userList = await Api.GetAllUsers();
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

            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            var currentUserIdStr = authState.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                           ?? authState.User.FindFirst("sub")?.Value;

            if (string.Equals(currentUserIdStr, selectedUser.Id.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                await PopupService.AlertAsync("Du kan ikke slette din egen brugerkonto.");
                return;
            }

            bool confirmed = await PopupService.ConfirmAsync(
                $"Er du sikker på, at du vil slette {selectedUser.Name}?");

            if (!confirmed) return;


            try
            {
                await Api.DeleteUser(selectedUser.Id);

                isEditing = false;
                selectedUser = null;
                editModel = null;

                await LoadUsers();
            }
            catch (Exception ex)
            {

                //Snackbar
            }
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
