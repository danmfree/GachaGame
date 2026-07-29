using GachaGame.Models;
using System.Collections.Generic;
using System.Linq;

namespace GachaGame.Systems
{
    public class HistorySystem
    {
        //private readonly List<PullHistory> history = new();
        private readonly PlayerData playerData;
        private int totalPulls = 0;

        public HistorySystem(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        public List<PullHistory> History
        {
            get
            {
                return playerData.History;
            }
        }

        //public IReadOnlyList<PullHistory> History => playerData.History;

        public void AddPull(Character pulled)
        {
            totalPulls++;

            int? pity = null;

            if (pulled.Rarity == Rarity.FiveStar)
                pity = pulled.PulledAtPity5;
            else if (pulled.Rarity == Rarity.FourStar)
                pity = pulled.PulledAtPity4;

            playerData.History.Add(new PullHistory(pulled, totalPulls, pity));

            // Keep only the newest 400 pulls
            if (playerData.History.Count > 400)
                playerData.History.RemoveAt(0);
        }
    }
}