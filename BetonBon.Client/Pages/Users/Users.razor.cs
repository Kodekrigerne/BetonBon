using BetonBon.Client.Shared.UserModels;
using BetonBon.Client.Shared.UserModels.ViewModels;
using BetonBon.Shared.Models.UserModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BetonBon.Client.Pages.Users
{
    public partial class Users
    {
        private UserViewModel? selectedUser;
        private UpdateUserModel? editModel;
        private List<UserViewModel>? userViewModels;

        private List<UserResponse>? allUsers;

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
            allUsers = await Api.GetAllUsers();
            userViewModels = allUsers.Select(u => new UserViewModel(u.Id, u.Username, u.Role, u.EmployeeNumber)).ToList();
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

            var currentUserIdStr = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
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
            catch (Exception)
            {
                await PopupService.AlertAsync("Fejl: Brugeren blev ikke slettet.");
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

        private void HandleEditUser(Guid id)
        {
            var user = allUsers?.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                selectedUser = userViewModels?.FirstOrDefault(u => u.Id == id);

                editModel = new UpdateUserModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role,
                    RowVersion = user.RowVersion
                };
                isEditing = true;
            }
        }
    }
}
