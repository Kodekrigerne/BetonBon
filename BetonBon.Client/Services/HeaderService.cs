using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Services
{
    public class HeaderService : IDisposable
    {
        public RenderFragment? Left { get; private set; }
        public RenderFragment? Center { get; private set; }
        public RenderFragment? Right { get; private set; }

        public event Action? OnChange;

        public void Set(RenderFragment? left = null, RenderFragment? center = null, RenderFragment? right = null)
        {
            Left = left;
            Center = center;
            Right = right;
            OnChange?.Invoke();
        }

        public void Clear()
        {
            Left = null;
            Center = null;
            Right = null;
            OnChange?.Invoke();
        }

        public void Dispose()
        {
            OnChange = null;
        }
    }
}
