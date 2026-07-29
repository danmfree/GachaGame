using System.Collections.Generic;
using GachaGame.Models;
using System;
using System.Linq;

namespace GachaGame.Models
{
    public class PlayerData
    {
        public int MoonTears { get; set; } = 12800;

        public int Pity5 { get; set; }

        public int Pity4 { get; set; }

        public bool GuaranteedFeatured { get; set; }

        public DateTime? LastDailyReward { get; set; }

        public List<OwnedCharacter> Characters { get; set; } = new();

        public List<PullHistory> History { get; set; } = new();

        public int TotalPulls { get; set; }
    }
}