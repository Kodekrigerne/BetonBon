using System;
using System.Collections.Generic;
using System.Text;
using BetonBon.Shared.Models.UserModels;

namespace BetonBon.Application.Users.UserQueries
{
    public record GetAllUsersQuery : IQuery<List<UserDto>>;
}
