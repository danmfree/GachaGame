using GachaGame.Models;
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace GachaGame.UI
{
    public class SummonAnimation
    {
        private const string AnimationPath =
            "Assets/Animations/MoonRift";


        public void PlayMoonRift()
        {
            for (int i = 1; i <= 20; i++)
            {
                Console.Clear();

                string file =
                    Path.Combine(
                        AnimationPath,
                        $"frame{i:D2}.txt"
                    );


                if (File.Exists(file))
                {
                    Console.WriteLine(File.ReadAllText(file));
                }
                else
                {
                    Console.WriteLine(
                        $"Missing frame: {file}");
                }

                Thread.Sleep(200);
            }
        }

        public void PlayRarityReveal(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.ThreeStar:
                    PlayAnimation("BlueReveal");
                    break;

                case Rarity.FourStar:
                    PlayAnimation("PurpleReveal");
                    break;

                case Rarity.FiveStar:
                    PlayAnimation("GoldReveal");
                    break;
            }
        }

        private void PlayAnimation(string folder)
        {
            string path =
                $"Assets/Animations/{folder}";

            var frames = Directory
                .GetFiles(path, "frame*.txt")
                .OrderBy(x => x);


            foreach (var frame in frames)
            {
                Console.Clear();

                Console.WriteLine(
                    File.ReadAllText(frame)
                );

                Thread.Sleep(150);
            }
        }

        public void PlaySummon(Rarity rarity)
        {
            PlayMoonRift();

            Thread.Sleep(500);

            PlayRarityReveal(rarity);
        }

        private readonly Dictionary<string, string> splashArtFiles = new()
        {
            { "Maria, the goddess of pain", "Maria.txt" },

            { "Aaron, the potato God", "Aaron.txt" },
            { "Natsuki Subaru, the superman complex", "NatsukiSubaru.txt" },
            { "SameSaturn, the larping one", "SameSaturn.txt" },
            { "AbyssalDream, the silly one", "AbyssalDream.txt" },

            { "Berk the Stalwart", "Berk.txt" },
            { "Lira Windrunner", "Lira.txt" },
            { "Talon Ashblade", "Talon.txt" },
            { "Mira Frostveil", "Mira.txt" },
            { "Doran Stonefist", "Doran.txt" },
            { "Kael Ironheart", "Kael.txt" },
            { "Sylva Moonshade", "Sylva.txt" },
            { "Ronan Emberforge", "Ronan.txt" },
            { "Nyra Stormcaller", "Nyra.txt" },
            { "Garrick Thornhelm", "Garrick.txt" },

            { "Farmhand Joji", "FarmhandJoji.txt" },
            { "Squire Beb", "SquireBeb.txt" },
            { "Apprentice Coll", "ApprenticeColl.txt" },
            { "Wanderer Fen", "WandererFen.txt" },
            { "Scout Rill", "ScoutRill.txt" },
            { "Peddler Oskar", "PeddlerOskar.txt" }
        };

        public void ShowSplashArt(Character character, int currentPull, int totalPulls)
        {
            Console.Clear();

            Console.WriteLine($"========== Pull {currentPull} of {totalPulls} ==========");
            Console.WriteLine();


            if (!splashArtFiles.ContainsKey(character.Name))
            {
                Console.WriteLine("No splash art found for:");
                Console.WriteLine(character.Name);
                return;
            }

            string file =
                Path.Combine(
                    "Assets",
                    "SplashArt",
                    splashArtFiles[character.Name]
                );


            if (File.Exists(file))
            {
                Console.WriteLine(File.ReadAllText(file));
            }
            else
            {
                Console.WriteLine($"Missing splash art: {file}");
            }

            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public void PlaySummon(List<Character> characters)
        {
            PlayMoonRift();

            Thread.Sleep(500);

            Character highestRarityCharacter =
                characters
                .OrderByDescending(x => x.Rarity)
                .First();

            PlayRarityReveal(highestRarityCharacter.Rarity);

            Thread.Sleep(500);

            for (int i = 0; i < characters.Count; i++)
            {
                ShowSplashArt(
                    characters[i],
                    i + 1,
                    characters.Count
                );
            }
        }

        //public void ShowMultipleSplashArts(List<Character> characters)
        //{
        //    foreach (var character in characters)
        //    {
        //        ShowSplashArt(character);
        //    }
        //}
    }
}