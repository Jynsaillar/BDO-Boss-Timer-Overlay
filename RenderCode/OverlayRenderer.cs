using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
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

        private List<int> _nextSpawnsIds = new List<int>();
        private List<string> _renderStrings = new List<string>();

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

            using (Stream resourceStream = currentAssembly.GetManifestResourceStream(@"Boss_Timer_Overlay.unknown.png"))
            {
                foreach (var bossName in Boss_Timer_Overlay.StaticData.BossInfo.Bosses)
                {
                    var imagePath = Path.Combine("./", bossName, ".png");
                    if (!File.Exists(imagePath))
                    {
                        //todo: Load default image
                        OverlayWindow.Graphics.LoadBitmapFromResource(resourceStream);
                        continue;
                    }
                    OverlayWindow.Graphics.LoadBitmap(imagePath);
                }
            }
        }

        public void ClearRenderStrings()
        {
            _renderStrings.Clear();
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

            if (_nextSpawnsIds.Count != _renderStrings.Count)
            {
                OverlayWindow.Graphics.EndScene();
                return;
            }

            // Draw segment; Place draw code here

            float screenWidth = 1920f;
            float screenHeight = 1080f;
            float imgDimensions = 90f; // 90x90
            float xStart = screenWidth - imgDimensions - 20;
            float yStart = 195f;
            int xOffset = 0;
            int yOffset = 10;

            for (int i = 0; i < _nextSpawnsIds.Count; i++)
            {
                // Boss images
                OverlayWindow.Graphics.DrawBitmap(

                    bitmapIndex: _nextSpawnsIds[i],
                    left: xStart,
                    top: yStart,
                    right: xStart + i * imgDimensions + xOffset,
                    bottom: yStart + i * imgDimensions + yOffset);

                // Strings with boss name, spawn time, time until spawn

                // Outline:
                OverlayWindow.Graphics.DrawText(

                    text: _renderStrings[i],
                    font: _outlineFont,
                    brush: _blackBrush,
                    x: 1605 + 1 + xOffset,
                    y: 300 + 1 + i * yOffset,
                    bufferText: false);

                // Actual text:
                OverlayWindow.Graphics.DrawText(

                    text: _renderStrings[i],
                    font: _outlineFont,
                    brush: _blackBrush,
                    x: 1605 + xOffset,
                    y: 300 + i * yOffset,
                    bufferText: false);
            }

            //OverlayWindow.Graphics.DrawBitmaps(1810f, 295f, 1810f + 90f, 1080 - (295f + 90f), 0, 100); // x, y, x+imageWidth, y+imageHeight, x offset for each following bitmap, y offset for each following bitmap

            OverlayWindow.Graphics.EndScene();
        }

        public void SetNextSpawns(int[] nextSpawns)
        {
            _nextSpawnsIds.Clear();

            if (nextSpawns.Length != 2)
            {
                return;
            }

            _nextSpawnsIds.AddRange(nextSpawns);
        }

        public void SetNextSpawns(List<int> nextSpawns)
        {
            SetNextSpawns(nextSpawns.ToArray());
        }

        public void AddRenderString(string renderString)
        {
            _renderStrings.Add(renderString);
        }

        public void SetFont(string fontName, int fontSize)
        {
            _font = OverlayWindow.Graphics.CreateFont(fontName, fontSize, true);
            _outlineFont = OverlayWindow.Graphics.CreateFont(fontName, fontSize, true, false);
        }

        public void AddBitmap(string filePath)
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
