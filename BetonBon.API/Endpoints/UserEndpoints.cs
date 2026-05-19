using BetonBon.Application;
using BetonBon.Application.Authentication;
using BetonBon.Application.Users;
using BetonBon.Application.Users.UserQueries;
using BetonBon.Shared.Enums;
using BetonBon.Shared.Models.UserModels;
using Refit;
using System.Security.Authentication;

namespace BetonBon.API.Endpoints
{
    public static class UserEndpoints
    {
        extension(WebApplication app)
        {
            public void MapUserEndpoints()
            {
                app.MapPost("/createUser", async (ICommandDispatcher commandDispatcher, CreateUserRequest userToCreate) =>
                {
                    try
                    {
                        var command = new CreateUserCommand(userToCreate.Username, userToCreate.Password, userToCreate.Role, userToCreate.EmployeeNumber);

                        var id = await commandDispatcher.DispatchAsync<CreateUserCommand, Guid>(command);

                        return Results.Ok(id);
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                        detail: ex.Content,
                        statusCode: (int)ex.StatusCode
                        );
                    }

                })
                .RequireAuthorization(nameof(UserRole.Admin));


                app.MapDelete("/deleteUser/{id}", async (ICommandDispatcher commandDispatcher, Guid id) =>
                {
                    try
                    {
                        var command = new DeleteUserCommand(id);

                        await commandDispatcher.DispatchAsync(command);

                        return Results.NoContent();
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                        detail: ex.Content,
                        statusCode: (int)ex.StatusCode
                        );
                    }
                })
                .RequireAuthorization(nameof(UserRole.Admin));


                app.MapGet("/viewUsers", async (IQueryDispatcher dispatcher) =>
                {
                    try
                    {
                        var users = await dispatcher.DispatchAsync<GetAllUsersQuery, List<UserDto>>(new GetAllUsersQuery());

                        return Results.Ok(users);
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                        detail: ex.Content,
                        statusCode: (int)ex.StatusCode
                        );
                    }
                })
                .RequireAuthorization(nameof(UserRole.Admin));


                app.MapPut("/updateUser", async (ICommandDispatcher commandDispatcher, UpdateUserRequest dto) =>
                {
                    try
                    {
                        var command = new UpdateUserCommand(dto.Id, dto.Username, dto.Password, dto.Role);

                        await commandDispatcher.DispatchAsync(command);

                        return Results.NoContent();
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                        detail: ex.Content,
                        statusCode: (int)ex.StatusCode
                        );
                    }
                })
                .RequireAuthorization(nameof(UserRole.Admin));


                app.MapPost("/login", async (IQueryDispatcher queryDispatcher, UserLoginDto userLogin) =>
                {
                    try
                    {
                        var query = new LoginQuery(userLogin.Username, userLogin.Password);

                        var response = await queryDispatcher.DispatchAsync<LoginQuery, LoginResponse>(query);

                        return Results.Ok(response);
                    }

                    catch (AuthenticationException)
                    {
                        return Results.Unauthorized();
                    }
                });

                app.MapPost("/refresh", async (IQueryDispatcher queryDispatcher, RefreshTokenRequest request) =>
                {
                    try
                    {
                        var query = new RefreshTokenQuery(request.Token, request.RefreshToken);
                        var response = await queryDispatcher.DispatchAsync<RefreshTokenQuery, LoginResponse>(query);
                        return Results.Ok(response);
                    }
                    catch (AuthenticationException)
                    {
                        return Results.Unauthorized();
                    }
                });
            }
        }
    }
}
