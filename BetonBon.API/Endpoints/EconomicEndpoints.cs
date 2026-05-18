using BetonBon.API.RefitInterfaces;
using BetonBon.Shared.Models;

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
            }
        }
    }
}
