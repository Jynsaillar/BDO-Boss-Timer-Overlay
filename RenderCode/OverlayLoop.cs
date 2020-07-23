using System.Linq;
using Overlay.NET.Common;
using Process.NET;
using Process.NET.Memory;

namespace Boss_Timer_Overlay.RenderCode
{
    public class OverlayLoop
    {
        private OverlayRenderer _overlayRenderer;
        private ProcessSharp _processSharp;

        private bool _halt = false;

        public void RunOverlay()
        {
            _halt = false;

            var process = System.Diagnostics.Process.GetProcessesByName("BlackDesert64").FirstOrDefault();
            int fps = 60;

            _overlayRenderer = new OverlayRenderer();
            _processSharp = new ProcessSharp(process, MemoryType.Remote);

            var d3DOverlay = (OverlayRenderer)_overlayRenderer;
            d3DOverlay.Settings.Current.UpdateRate = 1000 / fps;
            _overlayRenderer.Initialize(_processSharp.WindowFactory.MainWindow);
            _overlayRenderer.Enable();

            // Log some info about the overlay.
            Log.Debug("Starting update loop (open the process you specified and drag around)");
            Log.Debug("Update rate: " + d3DOverlay.Settings.Current.UpdateRate.Milliseconds());

            var info = d3DOverlay.Settings.Current;

            Log.Debug($"Author: {info.Author}");
            Log.Debug($"Description: {info.Description}");
            Log.Debug($"Name: {info.Name}");
            Log.Debug($"Identifier: {info.Identifier}");
            Log.Debug($"Version: {info.Version}");

            Log.Info("Note: Settings are saved to a settings folder in your main app folder.");

            Log.Info("Give your window focus to enable the overlay (and unfocus to disable..)");

            while (_halt != true)
            {
                _overlayRenderer.Update();
            }
        }

        public void StopOverlay()
        {
            _halt = true;
        }

        public void UpdateRenderString(string renderString)
        {
            _overlayRenderer.SetRenderString(renderString);
        }

        public void SetFont(string fontName, int fontSize)
        {
            _overlayRenderer.SetFont(fontName, fontSize);
        }

        public void SetBitmap(string filePath)
        {
            _overlayRenderer.SetBitmap(filePath);
        }
    }
}
