using System.Collections.Generic;
using GachaGame.Models;
using System;
using System.Linq;

namespace GachaGame.Models
{
    public class PlayerData
    {
        public int MoonTears { get; set; } = 25600;

        public int LimitedPity5 { get; set; }

        public int LimitedPity4 { get; set; }

        public bool LimitedGuaranteedFeatured { get; set; }

        public int StandardPity5 { get; set; } = 0;

        public int StandardPity4 { get; set; } = 0;

        public int LimitedPullCount { get; set; }

        public int StandardPullCount { get; set; }

        public DateTime? LastDailyReward { get; set; }

        public List<OwnedCharacter> Characters { get; set; } = new();

        public List<PullHistory> LimitedHistory { get; set; } = new();

        public List<PullHistory> StandardHistory { get; set; } = new();

        public int MoonTrialAttempts { get; set; } = 0;

        public DateTime LastMoonTrialDate { get; set; } = DateTime.MinValue;

        public int TotalPulls { get; set; }
    }
}