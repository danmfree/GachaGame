using GachaGame.Models;
using GachaGame.Models;
using System.Collections.Generic;

namespace GachaGame.Systems
{
    public class HistorySystem
    {
        private readonly List<PullHistory> history = new();
        private int totalPulls = 0;

        public IReadOnlyList<PullHistory> History => history;

        public void AddPull(Character pulled)
        {
            totalPulls++;

            int? pity = null;

            if (pulled.Rarity == Rarity.FiveStar)
                pity = pulled.PulledAtPity5;
            else if (pulled.Rarity == Rarity.FourStar)
                pity = pulled.PulledAtPity4;

            history.Add(new PullHistory(pulled, totalPulls, pity));

            // Keep only the newest 400 pulls
            if (history.Count > 400)
                history.RemoveAt(0);
        }
    }
}