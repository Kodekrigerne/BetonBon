using Microsoft.JSInterop;

namespace BetonBon.Client.Services
{
    public class LocalStorage
    {
        private readonly IJSRuntime _js;

        public LocalStorage(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<string?> LoadAsync(string key)
        {
            return await _js.InvokeAsync<string?>("storage.load", key);
        }

        public async Task SaveAsync(string key, string json)
        {
            await _js.InvokeVoidAsync("storage.save", key, json);
        }

        public async Task RemoveAsync(string key)
        {
            await _js.InvokeVoidAsync("storage.remove", key);
        }
    }
}
