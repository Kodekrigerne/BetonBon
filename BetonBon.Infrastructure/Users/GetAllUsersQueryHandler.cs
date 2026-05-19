using BetonBon.Application;
using BetonBon.Application.Users.UserQueries;
using BetonBon.Shared.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace BetonBon.Infrastructure.Users
{
    // Mikkel Klitgaard

    /// <summary>
    /// Handles queries to retrieve all users from the data store and returns a list of user data transfer objects.
    /// </summary>
    /// <remarks>This handler is used in query processing pipelines to fetch user information. 
    /// The returned list contains user details without tracking changes in the database context.</remarks>

    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, List<UserResponse>>
    {
        private readonly BetonBonDbContext _db;

        public GetAllUsersQueryHandler(BetonBonDbContext db)
        {
            _db = db;
        }

        async Task<List<UserResponse>?> IQueryHandler<GetAllUsersQuery, List<UserResponse>>.
            HandleAsync(GetAllUsersQuery query)
        {
            return await _db.Users
                .AsNoTracking()
                .Select(u => new UserResponse(u.Id, u.Username, u.Role, u.EmployeeNumber, u.RowVersion))
                .ToListAsync();
        }
    }
}
