using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Boss_Timer_Overlay.ManagerClasses
{
    public class BossData
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
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

        /*
        public static BossData GetBossData(string webUrl)
        {
            var bossDataNodes = _getBossData(webUrl);

            var bossName = RemoveLineBreaksAndSpaces(bossDataNodes.BossNameNode.InnerText);
            var bossImagePath = RootUrlWithoutLastSegment(webUrl) + RemoveLineBreaksAndSpaces(bossDataNodes.BossImageNode.Attributes["src"].Value);
            var bossTimeUntilSpawn = RemoveLineBreaksAndSpaces(bossDataNodes.BossTimerNode.InnerText);

            var bossData = new BossData()
            {
                Name = bossName,
                ImagePath = bossImagePath,
                TimeUntilSpawn = TimeSpan.ParseExact(bossTimeUntilSpawn, @"hh\:mm\:ss", System.Globalization.CultureInfo.InvariantCulture),
            };
            bossData.TimeUntilSpawn = new TimeSpan(RoundUp(DateTime.MinValue.Add(bossData.TimeUntilSpawn), TimeSpan.FromMinutes(1)).Ticks);
            bossData.NextSpawnTime = RoundUp(DateTime.Now.Add(bossData.TimeUntilSpawn), TimeSpan.FromMinutes(15));
            bossData.TimeUntilSpawn = bossData.TimeUntilSpawn.Add(TimeSpan.FromMinutes(1));

            return bossData;
        }
        */

        public static string ParseBossDataFromJObject(JObject jobject)
        {
            dynamic bossData = jobject;

            // Local function for lazily listing all days in the JSON object (if they exist)
            IEnumerable<string> getNameOfDays()
            {
                foreach (var day in (JObject)jobject["days"])
                {
                    yield return day.Key;
                }
            }

            if ((JObject)jobject["days"] is null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();

            var days = (JObject)jobject["days"];

            foreach (var day in days)
            {
                stringBuilder.AppendLine(day.Key);

                foreach (var timeSlot in (JObject)days[day.Key])
                {
                    stringBuilder.AppendLine(timeSlot.Key);

                    foreach (var boss in (JArray)timeSlot.Value)
                    {
                        stringBuilder.AppendLine(boss.Value<string>());
                    }
                }

                stringBuilder.AppendLine();
                /*
                foreach (var timeSlot in (JArray)day.Value)
                {
                    stringBuilder.AppendLine(timeSlot.Value<string>());

                    foreach (var boss in (JObject)jobject["days"][day.Key][timeSlot.Key])
                    {
                        stringBuilder.AppendLine(boss.Key);
                    }
                }
                */
            }

            return stringBuilder.ToString();

            //return string.Join(",", getNameOfDays());

            return string.Empty;
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

        public static string GetCurrentWorkingDirectory()
        {
            var currentAppPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            return System.IO.Path.GetDirectoryName(currentAppPath);
        }
    }
}
