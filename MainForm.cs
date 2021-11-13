﻿using Boss_Timer_Overlay.ManagerClasses;
using Boss_Timer_Overlay.RenderCode;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;

namespace Boss_Timer_Overlay
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void buttonListOpenProcesses_Click(object sender, EventArgs e)
        {
            textBoxProcesses.Text = string.Join("\r\n",
                GeneralManager.ListProcesses().OrderByDescending(x => x)
                );
        }

        private void buttonRunOverlay_Click(object sender, EventArgs e)
        {
            if (RenderManager.RendererIsRunning)
            {
                RenderManager.StopRenderer();

                buttonToggleOverlay.Text = "Show Overlay";

                //if (buttonUpdateBossInfo.Enabled == true)
                //    buttonUpdateBossInfo.Enabled = false;
            }
            else
            {
                RenderManager.StartRenderer();
                // Calling Update() immediately after starting the renderer causes a race condition since at this point
                // the render queue is still being initiated, configured, images are being loaded etc.
                // Either make this async or simply wait until the next timer tick updates the UI
                //RenderManager.Update();

                buttonToggleOverlay.Text = "Hide Overlay";

                //if (buttonUpdateBossInfo.Enabled == false)
                //    buttonUpdateBossInfo.Enabled = true;
            }
        }

        private void buttonDebugJson_Click(object sender, EventArgs e)
        {
            textBoxDebug.Text = GeneralManager.ParseBossDataTableFromJObject(IoManager.JObjectFromArgs());
        }

        //private void buttonUpdateBossInfo_Click(object sender, EventArgs e)
        //{
        //    if (RenderManager.RendererIsRunning == false)
        //        return;

        //    var bossData = GeneralManager.GetBossData("https://mmotimer.com/bdo/streamwidget/stream.php?server=eu");
        //    var bossInfoString = GeneralManager.BossDataToInfoString(bossData);

        //    var bossImagePath = GeneralManager.DownloadImage(bossData.ImageUrl, $"{bossData.Name}");

        //    if (File.Exists(bossImagePath))
        //    {
        //        RenderManager.SetBitmapImage(bossImagePath);
        //    }

        //    RenderManager.SetRenderFont("Georgia", 12);
        //    RenderManager.UpdateRenderString(bossInfoString);
        //}
    }
}
