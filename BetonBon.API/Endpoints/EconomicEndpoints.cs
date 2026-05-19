using BetonBon.API.RefitInterfaces;
using BetonBon.Shared.Enums;
using BetonBon.Shared.Models;
using BetonBon.Shared.Models.TimeEntries;
using Refit;

namespace BetonBon.API.Endpoints
{
    public static class EconomicEndpoints
    {
        extension(WebApplication app)
        {
            public void MapEconomicEndpoints()
            {
                // Get all projects
                app.MapGet("/api/projects", async (IEconomicProjectsApi economicApi, CancellationToken ct) =>
                {
                    try
                    {
                        var response = await economicApi.GetProjectsAsync(ct);
                        return Results.Ok(response.Items);
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                        detail: ex.Content,
                        statusCode: (int)ex.StatusCode
                        );
                    }
                });


                app.MapGet("/api/activitiesByProjectNumber", async (IEconomicProjectsApi economicApi, int projectNumber, CancellationToken ct) =>
                {

                    try
                    {
                        var initialResponse = await economicApi.GetProjectActivitiesAsync(projectNumber, ct);

                        var projectActivities = initialResponse.Items ?? [];

                        List<ActivityDTO> activities = [];

                        foreach (var activity in projectActivities)
                        {
                            activities.Add(economicApi.GetActivityByNumberAsync(activity.ActivityNumber, ct).Result);
                        }

                        return Results.Ok(activities);
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                            detail: ex.Content,
                            statusCode: (int)ex.StatusCode
                            );
                    }
                })
                .RequireAuthorization();


                app.MapGet("/api/materials", async (IEconomicProjectsApi economicApi, CancellationToken ct) =>
                {
                    try
                    {
                        var response = await economicApi.GetAllMaterialsAsync(ct);

                        return Results.Ok(response.Items);
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                           detail: ex.Content,
                           statusCode: (int)ex.StatusCode
                           );
                    }
                }).RequireAuthorization();



                app.MapPost("/api/newDraftEntry", async (IEconomicJournalsApi economicApi, NewDraftEntryDTO entry, CancellationToken ct) =>
                {
                    try
                    {
                        var creationResponse = await economicApi.PostNewEntryAsync(entry, ct);

                        BookEntryNumberDTO entryNumber = new()
                        {
                            EntryNumbers = [creationResponse.EntryNumber]
                        };

                        var response = await economicApi.BookDraftEntryAsync(entryNumber, ct);
                        return Results.Ok(response.StatusCode);
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                            detail: ex.Content,
                            statusCode: (int)ex.StatusCode
                            );
                    }

                })
                .RequireAuthorization();


                app.MapPost("/api/newTimeEntry", async (TimeEntry timeEntry, IEconomicProjectsApi economicApi, CancellationToken ct) =>
                {
                    try
                    {
                        var result = await economicApi.CreateTimeEntryAsync(timeEntry, ct);
                        return Results.Ok(result); //TODO: Results.Created(GET, result);
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                        detail: ex.Content,
                        statusCode: (int)ex.StatusCode
                        );
                    }
                })
                .RequireAuthorization();


                app.MapGet("/api/employees", async (IEconomicProjectsApi economicApi) =>
                {
                    try
                    {
                        var response = await economicApi.GetEmployeesAsync();
                        return Results.Ok(response);
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                        detail: ex.Content,
                        statusCode: (int)ex.StatusCode
                        );
                    }
                }).RequireAuthorization(nameof(UserRole.Admin));

                app.MapGet("api/timeEntries", async (int projectNumber, int activityNumber, int employeeNumber, IEconomicProjectsApi economicApi, CancellationToken ct) =>
                {
                    try
                    {
                        var response = await economicApi.GetTimeEntriesAsync(projectNumber, activityNumber, employeeNumber);
                        return Results.Ok(response.Items);
                    }
                    catch (ApiException ex)
                    {
                        return Results.Problem(
                            detail: ex.Content,
                            statusCode: (int)ex.StatusCode
                            );
                    }

                }).RequireAuthorization();
            }
        }
    }
}
