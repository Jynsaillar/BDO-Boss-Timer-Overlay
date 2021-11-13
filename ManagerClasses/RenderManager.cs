using Boss_Timer_Overlay.ManagerClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;

namespace Boss_Timer_Overlay.RenderCode
{
    public static class RenderManager
    {
        private static readonly OverlayLoop _overlayLoop;

        public static Thread _renderThread;
        public static bool RendererIsRunning = false;
        public static System.Timers.Timer UpdateTimer { get; set; }

        static RenderManager()
        {
            _overlayLoop = new OverlayLoop();
            UpdateTimer = new System.Timers.Timer()
            {
                Interval = GetTimerInterval(),
                AutoReset = false
            };
            UpdateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        private static double GetTimerInterval()
        {
            DateTime now = DateTime.Now;
            return ((60 - now.Second) * 1000 - now.Millisecond) *.10; //todo: remove debug timer downscaling
        }

        private static void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Update();
            UpdateTimer.Interval = GetTimerInterval();
            UpdateTimer.Start();
        }

        public static void Update()
        {
            if (RendererIsRunning == false)
                return;

            var upcomingBosses = GeneralManager.BossDataListFromJObject(IoManager.JObjectFromArgs());

            // Set font
            SetRenderFont("Georgia", 12);

            // Clear old RenderStrings
            ClearRenderStrings();

            List<int> nextSpawnIds = new List<int>();
            foreach (var upcomingBoss in upcomingBosses)
            {
                int bossId = Boss_Timer_Overlay.StaticData.BossInfo.GetBossIdFromName(upcomingBoss.Name);
                nextSpawnIds.Add(bossId);
                // Update RenderStrings
                AddRenderString(upcomingBoss.ToString());
            }

            SetNextSpawns(nextSpawnIds);
        }

        public static void StartRenderer()
        {
            if (_overlayLoop is null)
                return;

            if (_renderThread != null && _renderThread.IsAlive)
                return;

            _renderThread = new Thread(new ThreadStart(_overlayLoop.RunOverlay));
            _renderThread.Start();

            UpdateTimer.Start();

            RendererIsRunning = true;
        }

        public static void StopRenderer()
        {
            if (_overlayLoop is null)
                return;

            if (_renderThread is null)
                return;

            if (_renderThread.IsAlive == false)
                return;

            _overlayLoop.StopOverlay();

            _renderThread.Abort();

            UpdateTimer.Stop();

            RendererIsRunning = false;
        }

        public static void SetNextSpawns(List<int> nextSpawns)
        {
            if (nextSpawns == null || nextSpawns.Count != 2)
                return;

            _overlayLoop.SetNextSpawns(nextSpawns);
        }

        public static void ClearRenderStrings()
        {
            _overlayLoop.ClearRenderStrings();
        }

        public static void AddRenderString(string renderString)
        {
            _overlayLoop.AddRenderString(renderString);
        }

        public static void SetRenderFont(string fontName, int fontSize)
        {
            _overlayLoop.SetFont(fontName, fontSize);
        }

        public static void AddBitmapImage(string filePath)
        {
            _overlayLoop.AddBitmap(filePath);
        }
    }
}
