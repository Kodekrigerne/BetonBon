using System.ComponentModel.DataAnnotations;
using BetonBon.Shared.Enums;

namespace BetonBon.Client.Shared.UserModels
{
    //TODO: Det her er da vidst ikke en view model?
    public class UpdateUserModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Brugernavn skal udfyldes")]
        public string Username { get; set; } = string.Empty;

        public string? NewPassword { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}
