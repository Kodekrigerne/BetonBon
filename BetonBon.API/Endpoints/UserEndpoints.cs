using BetonBon.Application;
using BetonBon.Application.Authentication;
using BetonBon.Application.Users;
using BetonBon.Application.Users.UserQueries;
using BetonBon.Shared;
using BetonBon.Shared.Enums;
using BetonBon.Shared.Models.Authentication;
using BetonBon.Shared.Models.UserModels;
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
                    catch (Exception ex)
                    {
                        return Results.Problem(detail: ex.Message, statusCode: 500);
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
                    catch (Exception ex)
                    {
                        return Results.Problem(detail: ex.Message, statusCode: 500);
                    }
                })
                .RequireAuthorization(nameof(UserRole.Admin));


                app.MapGet("/viewUsers", async (IQueryDispatcher dispatcher) =>
                {
                    try
                    {
                        var users = await dispatcher.DispatchAsync<GetAllUsersQuery, List<UserResponse>>(new GetAllUsersQuery());

                        return Results.Ok(users);
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(detail: ex.Message, statusCode: 500);
                    }
                })
                .RequireAuthorization(nameof(UserRole.Admin));


                app.MapPut("/updateUser", async (ICommandDispatcher commandDispatcher, UpdateUserRequest request) =>
                {
                    try
                    {
                        var command = new UpdateUserCommand(request.Id, request.Username, request.Password, request.Role, request.RowVersion);

                        await commandDispatcher.DispatchAsync(command);

                        return Results.NoContent();
                    }
                    catch (ConcurrencyException ex)
                    {
                        return Results.Conflict(new { message = ex.Message });
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(detail: ex.Message, statusCode: 500);
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
