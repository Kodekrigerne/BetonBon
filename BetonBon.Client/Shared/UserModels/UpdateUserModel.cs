using BetonBon.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace BetonBon.Client.Shared.UserModels
{
    public class UpdateUserModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Brugernavn skal udfyldes")]
        public string Username { get; set; } = string.Empty;

        public string? NewPassword { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public uint RowVersion { get; set; }
    }
}
