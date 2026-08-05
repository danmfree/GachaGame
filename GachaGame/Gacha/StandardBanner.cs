using GachaGame.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GachaGame.Gacha
{
    public class StandardBanner
    {
        // How many pulls since last 5-star / 4-star
        //public int Pity5 = 0;
        //public int Pity4 = 0;

        //public bool GuaranteedFeatured = false; // True if lost 50/50

        private readonly PlayerData playerData;

        public StandardBanner(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        public const int HardPity5 = 80;
        public const int SoftPity5Start = 74;
        public const int HardPity4 = 10;

        static readonly List<Character> StandardFiveStars = new()
        {
            new Character("Aaron, the potato God", Rarity.FiveStar),
            new Character("Natsuki Subaru, the superman complex", Rarity.FiveStar),
            new Character("SameSaturn, the larping one", Rarity.FiveStar),
            new Character("AbyssalDream, the silly one", Rarity.FiveStar),
        };

        static readonly List<Character> FourStars = new()
        {
            new Character("Berk the Stalwart", Rarity.FourStar),
            new Character("Lira Windrunner", Rarity.FourStar),
            new Character("Talon Ashblade", Rarity.FourStar),
            new Character("Mira Frostveil", Rarity.FourStar),
            new Character("Doran Stonefist", Rarity.FourStar),
            new Character("Kael Ironheart", Rarity.FourStar),
            new Character("Sylva Moonshade", Rarity.FourStar),
            new Character("Ronan Emberforge", Rarity.FourStar),
            new Character("Nyra Stormcaller", Rarity.FourStar),
            new Character("Garrick Thornhelm", Rarity.FourStar),
        };

        static readonly List<Character> ThreeStars = new()
        {
            new Character("Farmhand Joji", Rarity.ThreeStar),
            new Character("Squire Beb", Rarity.ThreeStar),
            new Character("Apprentice Coll", Rarity.ThreeStar),
            new Character("Wanderer Fen", Rarity.ThreeStar),
            new Character("Scout Rill", Rarity.ThreeStar),
            new Character("Peddler Oskar", Rarity.ThreeStar),
        };

        static readonly Random Rng = new Random();

        public Character Pull()
        {
            playerData.StandardPity5++;
            playerData.StandardPity4++;

            double fiveStarChance = 0.006; // 0,006

            if (playerData.StandardPity5 >= SoftPity5Start)
            {
                int stepsIntoSoft = playerData.StandardPity5 - SoftPity5Start + 1;
                fiveStarChance = Math.Min(1.0, 0.006 + stepsIntoSoft * 0.15);
            }

            if (playerData.StandardPity5 >= HardPity5)
                fiveStarChance = 1.0;

            bool got5 = Rng.NextDouble() < fiveStarChance;

            if (got5)
            {
                Character baseChar =
    StandardFiveStars[Rng.Next(StandardFiveStars.Count)];

                Character c = new Character(baseChar.Name, baseChar.Rarity)
                {
                    PulledAtPity5 = playerData.StandardPity5
                };

                playerData.StandardPity5 = 0;
                playerData.StandardPity4 = 0;

                return c;
            }

            double fourStarChance = 0.051;
            bool guaranteed4 = playerData.StandardPity4 >= HardPity4;
            bool got4 = guaranteed4 || Rng.NextDouble() < fourStarChance;

            if (got4)
            {
                Character c = FourStars[Rng.Next(FourStars.Count)];

                c.PulledAtPity4 = playerData.StandardPity4;
                c.PulledAtPity5 = null;

                playerData.StandardPity4 = 0;

                return c;
            }

            return ThreeStars[Rng.Next(ThreeStars.Count)];
        }
    }   
}
