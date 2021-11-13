using System.Collections.Generic;

namespace Boss_Timer_Overlay.Internals
{
    public class BossOverlaySettings : OverlaySettings
    {
        private List<string> _bosses;

        public System.Collections.ObjectModel.ReadOnlyCollection<string> GetBosses()
        {
            return _bosses.AsReadOnly();
        }

        public BossOverlaySettings()
            : base()
        {
            _bosses = new List<string>() {
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
            };
        }
    }
}
