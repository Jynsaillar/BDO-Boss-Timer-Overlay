using System.Linq;

namespace Boss_Timer_Overlay.StaticData
{
    public static class BossInfo
    {
        public static System.Collections.ObjectModel.ReadOnlyCollection<string> Bosses = new System.Collections.Generic.List<string>() {
            "None",
            "Kzarka",
            "Karanda",
            "Kutum",
            "Nouver",
            "Offin",
            "Garmoth",
            "Vell",
            "Quint",
            "Muraka",
            "Black Shadow",
            "Event"
            }.AsReadOnly();

        public static int GetBossIdFromName(string bossName)
        {
            int id = Bosses
                .ToList()
                .FindIndex(x => x.Equals(bossName, System.StringComparison.OrdinalIgnoreCase));

            return (id >= 0) ? id : 0; // Return the id for "None" = 0 by default, since List.IndexOf(...) returns -1 if no match is found
        }
    }
}
