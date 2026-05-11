using System.ComponentModel.DataAnnotations;
using BetonBon.Shared.Enums;

namespace BetonBon.Client.Shared.ViewModels
{
    public class CreateNewUserModel
    {
        [Required(ErrorMessage = "Navn skal udfyldes")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Navnet skal være mellem 2 og 50 tegn")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Adgangskode skal udfyldes")]
        [MinLength(8, ErrorMessage = "Adgangskoden skal være mindst 8 tegn")]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Vælg venligst en rolle")]
        public UserRole Role { get; set; } = UserRole.User;
    }
}
