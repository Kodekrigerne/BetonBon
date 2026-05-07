using System;
using System.Collections.Generic;
using System.Text;
using BetonBon.Shared.Enums;

namespace BetonBon.Shared.Models
{
    public record UserDto(Guid Id, string Username, UserRole Role);

}
