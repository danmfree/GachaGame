using System.Collections.Generic;
using GachaGame.Gacha;
using GachaGame.Models;

namespace GachaGame.Systems
{
    public class GachaSystem
    {
        private readonly CurrencySystem currencySystem;

        private readonly LimitedBanner limitedBanner;
        private readonly StandardBanner standardBanner;

        private readonly CollectionSystem collectionSystem;
        private readonly HistorySystem historySystem;

        private readonly SaveSystem saveSystem;
        private readonly PlayerData playerData;


        public GachaSystem(
            CurrencySystem currencySystem,
            LimitedBanner limitedBanner,
            StandardBanner standardBanner,
            CollectionSystem collectionSystem,
            HistorySystem historySystem,
            SaveSystem saveSystem,
            PlayerData playerData)
        {
            this.currencySystem = currencySystem;

            this.limitedBanner = limitedBanner;
            this.standardBanner = standardBanner;

            this.collectionSystem = collectionSystem;
            this.historySystem = historySystem;

            this.saveSystem = saveSystem;
            this.playerData = playerData;
        }

        public List<Character> Pull(int amount, bool limitedBannerPull)
        {
            int cost = amount == 1 ? 160 : 1600;

            if (!currencySystem.Spend(cost))
            {
                return new List<Character>();
            }

            var results = new List<Character>();

            for (int i = 0; i < amount; i++)
            {
                if (limitedBannerPull)
                {
                    results.Add(limitedBanner.Pull());
                }
                else
                {
                    results.Add(standardBanner.Pull());
                }
            }

            foreach (var pulled in results)
            {
                historySystem.AddPull(pulled, limitedBannerPull);

                int duplicateReward = collectionSystem.AddCharacter(pulled);

                if (duplicateReward > 0)
                {
                    currencySystem.Add(duplicateReward);
                }
            }

            saveSystem.Save(playerData);

            return results;
        }
    }
}