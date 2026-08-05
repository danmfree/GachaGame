using GachaGame.Models;
using System.Collections.Generic;
using System.Linq;

namespace GachaGame.Systems
{
    public class HistorySystem
    {
        //private readonly List<PullHistory> history = new();
        private readonly PlayerData playerData;
        
        // Not needed as we save in playerData.cs
        //private int totalPulls = 0;

        public HistorySystem(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        // Old history property
        //public List<PullHistory> History
        //{
        //    get
        //    {
        //        return playerData.History;
        //    }
        //}

        //public IReadOnlyList<PullHistory> History => playerData.History;

        public void AddPull(Character pulled, bool limitedBanner)
        {
            int pullNumber;

            if (limitedBanner)
            {
                playerData.LimitedPullCount++;
                pullNumber = playerData.LimitedPullCount;
            }
            else
            {
                playerData.StandardPullCount++;
                pullNumber = playerData.StandardPullCount;
            }

            int? pity = null;

            if (pulled.Rarity == Rarity.FiveStar)
                pity = pulled.PulledAtPity5;
            else if (pulled.Rarity == Rarity.FourStar)
                pity = pulled.PulledAtPity4;

            List<PullHistory> history =
                limitedBanner
                ? playerData.LimitedHistory
                : playerData.StandardHistory;

            history.Add(new PullHistory(pulled, pullNumber, pity));
           
            if (history.Count > 400)
                history.RemoveAt(0);
        }

        public List<PullHistory> GetHistory(bool limitedBanner)
        {
            return limitedBanner
                ? playerData.LimitedHistory
                : playerData.StandardHistory;
        }
    }
}