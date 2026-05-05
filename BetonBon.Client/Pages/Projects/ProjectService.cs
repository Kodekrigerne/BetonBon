using System.Net.Http.Json;

namespace BetonBon.Client.Pages.Projects
{
    public class ProjectService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<List<ProjectDTO>> GetAllProjectsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProjectDTO>>("/api/projects") ?? [];
        }
    }
}