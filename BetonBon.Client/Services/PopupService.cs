using Microsoft.JSInterop;

namespace BetonBon.Client.Services
{
    public class PopupService
    {
        private readonly IJSRuntime _js;

        public PopupService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<bool> ConfirmAsync(string message)
        {
            return await _js.InvokeAsync<bool>("confirm", message);
        }


    }
}
