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
                app.MapGet("/api/projects", async (IEconomicProjectsRelayApi economicApi) =>
                {

                    var response = await economicApi.GetProjectsAsync();
                    return Results.Ok(response.Projects);
                })
                .RequireAuthorization();


                app.MapGet("/api/activitiesByProjectNumber", async (IEconomicProjectsRelayApi economicApi, int projectNumber) =>
                {
                    var initialResponse = await economicApi.GetProjectActivitiesAsync(projectNumber);

                    var projectActivities = initialResponse.ProjectActivities;

                    List<ActivityDTO> activities = [];

                    foreach (var activity in projectActivities)
                    {
                        activities.Add(economicApi.GetActivityByNumberAsync(activity.ActivityNumber).Result);
                    }

                    return Results.Ok(activities);
                })
                .RequireAuthorization();


                app.MapGet("/api/materials", async (IEconomicProjectsRelayApi economicApi) =>
                {
                    var response = await economicApi.GetAllMaterialsAsync();

                    return Results.Ok(response.Materials);
                });


                app.MapPost("/api/newDraftEntry", async (IEconomicJournalsRelayApi economicApi, NewDraftEntryDTO entry) =>
                {
                    var creationResponse = await economicApi.PostNewEntryAsync(entry);

                    BookEntryNumberDTO entryNumber = new([creationResponse.CreatedEntryNumber]);

                    var response = await economicApi.BookDraftEntryAsync(entryNumber);
                    return Results.Ok(response.StatusCode);

                });


                app.MapPost("/api/newTimeEntry", async (TimeEntry timeEntry, IEconomicProjectsRelayApi economicApi, CancellationToken ct) =>
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
                }).RequireAuthorization();


                app.MapGet("/api/employees", async (IEconomicProjectsRelayApi economicApi) =>
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
            }
        }
    }
}
