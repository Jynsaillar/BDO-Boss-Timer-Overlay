using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Boss_Timer_Overlay.ManagerClasses
{
    public class BossData
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public TimeSpan TimeUntilSpawn { get; set; }
        public DateTime NextSpawnTime { get; set; }

        public override string ToString()
        {
            return $"Boss: { this.Name}\r\n\r\nTime Until Spawn: { this.TimeUntilSpawn.ToString(@"hh\:mm")}\r\n\r\nSpawn Time: { this.NextSpawnTime.ToString("dddd, HH:mm")}";
        }
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

        public static List<BossData> BossDataListFromJObject(JObject jobject)
        {
            if ((JObject)jobject["days"] is null)
                return new List<BossData>();

            var days = (JObject)jobject["days"];

            // Find today
            var today = string.Empty;
            foreach (var day in days)
            {
                bool dayMatch = string.Equals(day.Key, DateTime.Today.DayOfWeek.ToString(), StringComparison.InvariantCultureIgnoreCase);

                if (dayMatch == false)
                    continue;

                today = day.Key;
            }

            var spawnHour = TimeSpan.MinValue;
            IEnumerable<TimeSpan> timeSlots()
            {
                foreach (var timeSlot in (JObject)days[today])
                {
                    spawnHour = DateTime.ParseExact(timeSlot.Key, "HH:mm", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay;
                    yield return spawnHour;
                }
            }

            /*
             * We want to figure out what the next two bosses to spawn will be.
             * Thus, we need the list of spawn times of today sorted from earliest to latest spawn time,
             * then find all time slots after or equal to right now,
             * then only pick the first two results.
             *
             */

            IEnumerable<string> upcomingSpawnSlots =
                timeSlots()
                .OrderBy(hourOfDay => hourOfDay.Hours)
                .Where(timeSlot => timeSlot >= DateTime.Now.TimeOfDay)
                .Take(2)
                .Select(time => time.ToString(@"hh\:mm"))
                .ToArray();

            IEnumerable<BossData> nextBosses()
            {
                foreach (var spawnSlot in upcomingSpawnSlots)
                {
                    foreach (var bossName in (JArray)(days[today][spawnSlot]))
                    {
                        var boss = new BossData();
                        boss.Name = bossName.ToString();
                        boss.NextSpawnTime = DateTime.Parse(spawnSlot);
                        boss.TimeUntilSpawn = DateTime.Now - boss.NextSpawnTime.Add(TimeSpan.FromMinutes(1));
                        boss.ImagePath = System.IO.File.Exists($"./{boss.Name}.png") ? $"./{boss.Name}.png" : "";

                        yield return boss;
                    }
                }
            }

            return nextBosses().ToList();
        }

        public static string ParseBossDataTableFromJObject(JObject jobject)
        {
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
            }

            return stringBuilder.ToString();
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
