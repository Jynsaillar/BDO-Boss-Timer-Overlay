using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using Boss_Timer_Overlay.Internals;
using Overlay.NET.Common;
using Overlay.NET.Directx;
using Process.NET.Windows;
using SharpDX;

namespace Boss_Timer_Overlay.RenderCode
{
    public class OverlayRenderer : DirectXOverlayPlugin
    {
        private readonly TickEngine _tickEngine = new TickEngine();
        public readonly ISettings<OverlaySettings> Settings = new SerializableSettings<OverlaySettings>();
        private int _displayFps;
        private int _font;
        private int _outlineFont;
        private int _hugeFont;
        private int _i;
        private int _interiorBrush;
        private int _redBrush;
        private int _blueBrush;
        private int _greenBrush;
        private int _yellowBrush;
        private int _blackBrush;
        private int _whiteBrush;
        private int _redOpacityBrush;
        private float _rotation;
        private Stopwatch _watch;

        private string _renderString = string.Empty;

        public override void Initialize(IWindow targetWindow)
        {
            // Set target window by calling the base method
            base.Initialize(targetWindow);

            // For demo, show how to use settings
            var current = Settings.Current;
            var type = GetType();

            if (current.UpdateRate == 0)
                current.UpdateRate = 1000 / 60;

            current.Author = GetAuthor(type);
            current.Description = GetDescription(type);
            current.Identifier = GetIdentifier(type);
            current.Name = GetName(type);
            current.Version = GetVersion(type);

            // File is made from above info
            Settings.Save();
            Settings.Load();

            OverlayWindow = new DirectXOverlayWindow(targetWindow.Handle, false);
            //_watch = Stopwatch.StartNew();

            _redBrush = OverlayWindow.Graphics.CreateBrush(System.Drawing.Color.Red);
            _blueBrush = OverlayWindow.Graphics.CreateBrush(System.Drawing.Color.Blue);
            _greenBrush = OverlayWindow.Graphics.CreateBrush(System.Drawing.Color.Green);
            _yellowBrush = OverlayWindow.Graphics.CreateBrush(System.Drawing.Color.Yellow);
            _blackBrush = OverlayWindow.Graphics.CreateBrush(System.Drawing.Color.Black);
            _whiteBrush = OverlayWindow.Graphics.CreateBrush(System.Drawing.Color.White);
            _redOpacityBrush = OverlayWindow.Graphics.CreateBrush(System.Drawing.Color.FromArgb(80, 255, 0, 0));
            _interiorBrush = OverlayWindow.Graphics.CreateBrush(0x7FFFFF00);

            _font = OverlayWindow.Graphics.CreateFont("Arial", 20);
            _outlineFont = OverlayWindow.Graphics.CreateFont("Arial", 20, false, true);
            _hugeFont = OverlayWindow.Graphics.CreateFont("Arial", 50, true);

            _rotation = 0.0f;
            _displayFps = 0;
            _i = 0;
            // Set up update interval and register events for the tick engine.

            _tickEngine.PreTick += OnPreTick;
            _tickEngine.Tick += OnTick;

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            using (Stream resourceStream = currentAssembly.GetManifestResourceStream(@"Boss_Timer_Overlay.updating.png"))
            {
                OverlayWindow.Graphics.LoadBitmapFromResource(resourceStream);
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (!OverlayWindow.IsVisible)
            {
                return;
            }

            OverlayWindow.Update();
            InternalRender();
        }

        private void OnPreTick(object sender, EventArgs e)
        {
            var targetWindowIsActivated = TargetWindow.IsActivated;
            if (!targetWindowIsActivated && OverlayWindow.IsVisible)
            {
                //_watch.Stop();
                ClearScreen();
                OverlayWindow.Hide();
            }
            else if (targetWindowIsActivated && !OverlayWindow.IsVisible)
            {
                OverlayWindow.Show();
            }
        }

        // ReSharper disable once RedundantOverriddenMember
        public override void Enable()
        {
            _tickEngine.Interval = Settings.Current.UpdateRate.Milliseconds();
            _tickEngine.IsTicking = true;
            base.Enable();
        }

        // ReSharper disable once RedundantOverriddenMember
        public override void Disable()
        {
            _tickEngine.IsTicking = false;
            base.Disable();
        }

        public override void Update() => _tickEngine.Pulse();

        protected void InternalRender()
        {
            OverlayWindow.Graphics.BeginScene();
            OverlayWindow.Graphics.ClearScene();

            // Draw segment; Place draw code here

            OverlayWindow.Graphics.DrawBitmap(1810f, 295f, 1810f + 90f, 295f + 90f); // x, y, x+imageWidth, y+imageHeight

            // Outline:
            OverlayWindow.Graphics.DrawText(_renderString, _outlineFont, _blackBrush, 1605 + 1, 300 + 1, false);
            // Actual text:
            OverlayWindow.Graphics.DrawText(_renderString, _font, _whiteBrush, 1605, 300, false);

            OverlayWindow.Graphics.EndScene();
        }

        public void SetRenderString(string renderString)
        {
            _renderString = renderString;
        }

        public void SetFont(string fontName, int fontSize)
        {
            _font = OverlayWindow.Graphics.CreateFont(fontName, fontSize, true);
            _outlineFont = OverlayWindow.Graphics.CreateFont(fontName, fontSize, true, false);
        }

        public void SetBitmap(string filePath)
        {
            // File size check, existing file of size 0 must be ignored!
            if (new System.IO.FileInfo(filePath).Length == 0)
                return;

            OverlayWindow.Graphics.LoadBitmap(filePath);
        }

        public override void Dispose()
        {
            OverlayWindow.Dispose();
            base.Dispose();
        }

        private void ClearScreen()
        {
            OverlayWindow.Graphics.BeginScene();
            OverlayWindow.Graphics.ClearScene();
            OverlayWindow.Graphics.EndScene();
        }
    }
}
