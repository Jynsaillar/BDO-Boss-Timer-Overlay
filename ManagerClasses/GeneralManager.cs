using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Boss_Timer_Overlay.ManagerClasses
{
    public class BossData
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public TimeSpan TimeUntilSpawn { get; set; }
        public DateTime NextSpawnTime { get; set; }
    }

    public static class GeneralManager
    {
        public static IEnumerable<string> ListProcesses()
        {
            System.Diagnostics.Process[] processCollection = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in processCollection)
            {
                yield return p.ProcessName;
            }
        }

        public static IEnumerable<string> ParseHtmlSourceFromWebUrl(string webUrl)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(webUrl);

            foreach (var node in htmlDoc.DocumentNode.SelectNodes("//body"))
            {
                yield return node.Name + ":\r\n" + node.OuterHtml;
            }
        }

        private static (HtmlNode RootNode, HtmlNode BossImageNode, HtmlNode BossNameNode, HtmlNode BossTimerNode) _getBossData(string webUrl)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(webUrl);

            var bossNameNode = htmlDoc.DocumentNode.SelectSingleNode("//td[contains(@class, 'stream-boss-title')]");

            var rootNode = bossNameNode.ParentNode;

            var bossImageNode = rootNode.SelectSingleNode("//td/img");

            var bossTimerNode = rootNode.SelectSingleNode("//div[@class=\"countdown\"]");

            return (rootNode, bossImageNode, bossNameNode, bossTimerNode);
        }

        public static string RootUrlWithoutLastSegment(string webUrl)
        {
            Uri uri = new Uri(webUrl);
            string lastSegment = webUrl.Split('/').Last();
            string webUrlWithLastSegmentRemoved = webUrl.Substring(0, webUrl.Length - lastSegment.Length);
            return webUrlWithLastSegmentRemoved;
        }

        public static BossData GetBossData(string webUrl)
        {
            var bossDataNodes = _getBossData(webUrl);

            var bossName = RemoveLineBreaksAndSpaces(bossDataNodes.BossNameNode.InnerText);
            var bossImageUrl = RootUrlWithoutLastSegment(webUrl) + RemoveLineBreaksAndSpaces(bossDataNodes.BossImageNode.Attributes["src"].Value);
            var bossTimeUntilSpawn = RemoveLineBreaksAndSpaces(bossDataNodes.BossTimerNode.InnerText);

            var bossData = new BossData()
            {
                Name = bossName,
                ImageUrl = bossImageUrl,
                TimeUntilSpawn = TimeSpan.ParseExact(bossTimeUntilSpawn, @"hh\:mm\:ss", System.Globalization.CultureInfo.InvariantCulture),
            };
            bossData.TimeUntilSpawn = new TimeSpan(RoundUp(DateTime.MinValue.Add(bossData.TimeUntilSpawn), TimeSpan.FromMinutes(1)).Ticks);
            bossData.NextSpawnTime = RoundUp(DateTime.Now.Add(bossData.TimeUntilSpawn), TimeSpan.FromMinutes(15));
            bossData.TimeUntilSpawn = bossData.TimeUntilSpawn.Add(TimeSpan.FromMinutes(1));

            return bossData;
        }

        public static string NextBossInfo(string webUrl)
        {
            var bossData = GetBossData(webUrl);
            return $"Next Boss: { bossData.Name}\r\n\r\nTime Until Spawn: { bossData.TimeUntilSpawn.ToString(@"hh\:mm")}\r\n\r\nSpawn Time: { bossData.NextSpawnTime.ToString("dddd, HH:mm")}";
        }

        public static string BossDataToInfoString(BossData bossData)
        {
            return $"Next Boss: { bossData.Name}\r\n\r\nTime Until Spawn: { bossData.TimeUntilSpawn.ToString(@"hh\:mm")}\r\n\r\nSpawn Time: { bossData.NextSpawnTime.ToString("dddd, HH:mm")}";
        }

        public static DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        public static string RemoveLineBreaksAndSpaces(string input)
        {
            return input.Replace(System.Environment.NewLine, "").Replace(" ", "");
        }

        public static string DownloadImage(string webUrl, string fileName)
        {
            var downloadedFilePath = Path.Combine(GetCurrentWorkingDirectory(), $"{fileName}.png");

            if (File.Exists(downloadedFilePath))
                return downloadedFilePath;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(webUrl, downloadedFilePath);
                    return downloadedFilePath;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetCurrentWorkingDirectory()
        {
            var currentAppPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            return System.IO.Path.GetDirectoryName(currentAppPath);
        }
    }
}
