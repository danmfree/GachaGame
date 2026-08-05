using GachaGame.Gacha;
using GachaGame.Models;
using GachaGame.Systems;
using System.Linq;
using System.Collections.Generic;

namespace GachaGame.UI
{
    public class Menu
    {
        private readonly CurrencySystem currencySystem;
        private readonly RewardSystem rewardSystem;

        private readonly LimitedBanner limitedBanner;
        private readonly StandardBanner standardBanner;

        private readonly CollectionSystem collectionSystem;
        private readonly HistorySystem historySystem;

        private readonly GachaSystem gachaSystem;
        private readonly PlayerData playerData;

        public Menu(
            CurrencySystem currencySystem,
            RewardSystem rewardSystem,
            LimitedBanner limitedBanner,
            StandardBanner standardBanner,
            CollectionSystem collectionSystem,
            HistorySystem historySystem,
            GachaSystem gachaSystem,
            PlayerData playerData)
        {
            this.currencySystem = currencySystem;
            this.rewardSystem = rewardSystem;

            this.limitedBanner = limitedBanner;
            this.standardBanner = standardBanner;

            this.collectionSystem = collectionSystem;
            this.historySystem = historySystem;

            this.gachaSystem = gachaSystem;
            this.playerData = playerData;
        }

        public void ShowCollection()
        {
            Console.Clear();

            Console.WriteLine("=== Character Collection ===");
            Console.WriteLine();

            if (collectionSystem.Collection.Count == 0)
            {
                Console.WriteLine("No characters pulled yet.");
            }
            else
            {
                foreach (var character in collectionSystem.Collection
                             .OrderByDescending(c => c.Character.Rarity))
                {
                    ConsoleRenderer.PrintCollectionCharacter(character);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press ESC to return...");

            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
            }
        }

        public void ShowBannerMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("========== BANNERS ==========");
                Console.WriteLine();
                Console.WriteLine("1. Event Character Banner");
                Console.WriteLine("2. Standard Banner");
                Console.WriteLine();

                Console.Write("Choice: ");

                string choice = GetMenuChoice();

                switch (choice)
                {
                    case "1":
                        ShowEventBannerMenu();
                        break;

                    case "2":
                        ShowStandardBannerMenu();
                        break;

                    case "ESC":
                        return;
                }
            }
        }

        public void ShowEventBannerMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("====== EVENT CHARACTER BANNER ======");
                Console.WriteLine();
                Console.WriteLine($"Moon Tears: {currencySystem.Amount}");
                Console.WriteLine();
                Console.WriteLine("1. Single Pull");
                Console.WriteLine("2. Pull x10");
                Console.WriteLine("3. History");
                Console.WriteLine();

                Console.Write("Choice: ");

                string choice = GetMenuChoice();

                switch (choice)
                {
                    case "1":
                        {
                            var results = gachaSystem.Pull(1, true);
                            ShowPullResults(results, 1, true);
                            break;
                        }

                    case "2":
                        {
                            var results = gachaSystem.Pull(10, true);
                            ShowPullResults(results, 10, true);
                            break;
                        }

                    case "3":
                        ShowHistory(true);
                        break;

                    case "ESC":
                        return;
                }
            }
        }

        public void ShowStandardBannerMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("====== STANDARD BANNER ======");
                Console.WriteLine();
                Console.WriteLine($"Moon Tears: {currencySystem.Amount}");
                Console.WriteLine();
                Console.WriteLine("1. Single Pull");
                Console.WriteLine("2. Pull x10");
                Console.WriteLine("3. History");
                Console.WriteLine();

                Console.Write("Choice: ");

                string choice = GetMenuChoice();

                switch (choice)
                {
                    case "1":
                        {
                            var results = gachaSystem.Pull(1, false);
                            ShowPullResults(results, 1, false);
                            break;
                        }

                    case "2":
                        {
                            var results = gachaSystem.Pull(10, false);
                            ShowPullResults(results, 10, false);
                            break;
                        }

                    case "3":
                        ShowHistory(false);
                        break;

                    case "ESC":
                        return;
                }
            }
        }

        private void ShowHistory(bool limitedBanner)
        {
            int page = 0;

            var history = historySystem.GetHistory(limitedBanner);

            while (true)
            {
                Console.Clear();

                int totalPages = Math.Max(1, (history.Count + 9) / 10);

                Console.WriteLine(limitedBanner
                    ? "===== Limited Banner History ====="
                    : "===== Standard Banner History =====");

                Console.WriteLine($"Page {page + 1}/{totalPages}");
                Console.WriteLine();

                var pageEntries = history
                    .AsEnumerable()
                    .Reverse()
                    .Skip(page * 10)
                    .Take(10);

                foreach (var h in pageEntries)
                {
                    string stars = new string('*', (int)h.Character.Rarity);

                    Console.Write($"#{h.PullNumber,-4} ");

                    Console.ForegroundColor = ConsoleRenderer.GetRarityColor(h.Character.Rarity);

                    Console.Write($"[{stars}] ");
                    Console.Write($"{h.Character.Name,-35}");

                    if (h.Pity.HasValue)
                        Console.Write($" Pity {h.Pity.Value}");

                    Console.ResetColor();

                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine("[A] Previous   [D] Next   [ESC] Back");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.A:
                        if (page > 0)
                            page--;
                        break;

                    case ConsoleKey.D:
                        if (page < totalPages - 1)
                            page++;
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        public string GetMenuChoice()
        {
            string input = "";

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                // ESC immediately goes back
                if (key.Key == ConsoleKey.Escape)
                {
                    return "ESC";
                }

                // Allow typing numbers
                if (char.IsDigit(key.KeyChar))
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }

                // Confirm with ENTER
                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return input;
                }

                // Backspace support
                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input = input.Substring(0, input.Length - 1);
                    Console.Write("\b \b");
                }
            }
        }

        // Helper
        private void ShowPullResults(List<Character> results, int amount, bool limitedBanner)
        {
            if (results.Count == 0)
            {
                Console.Clear();

                Console.WriteLine("Not enough Moon Tears!");
                Console.WriteLine();

                Console.WriteLine($"Moon Tears: {currencySystem.Amount}");

                Console.WriteLine();
                Console.Write("Press Enter to return...");
                Console.ReadLine();

                return;
            }


            Console.Clear();

            LoadingAnimation.ShowLoadingBar(results);


            if (amount == 1)
            {
                Console.Clear();

                Console.WriteLine("Single Pull");
                Console.WriteLine();

                ConsoleRenderer.PrintCharacter(results[0]);

                Console.WriteLine();
                Console.Write("Press Enter to continue...");
                Console.ReadLine();
            }
            else
            {
                ShowPullsOneByOne(results);
            }


            Console.Clear();

            ConsoleRenderer.ShowSummary(results, amount);

            Console.WriteLine();

            ShowPityStatus(limitedBanner);

            Console.WriteLine();
            Console.Write("Press Enter to return...");
            Console.ReadLine();
        }

        private void ShowPullsOneByOne(List<Character> results)
        {
            for (int i = 0; i < results.Count; i++)
            {
                Console.Clear();

                Console.WriteLine($"Pull {i + 1} of {results.Count}");
                Console.WriteLine();

                ConsoleRenderer.PrintCharacter(results[i]);

                Console.WriteLine();

                Console.Write(i < results.Count - 1
                    ? "Press Enter for next pull..."
                    : "Press Enter to see summary...");

                Console.ReadLine();
            }
        }

        private void ShowPityStatus(bool limitedBanner)
        {
            Console.WriteLine("===== Pity Status =====");
            Console.WriteLine();

            if (limitedBanner)
            {
                Console.WriteLine(
                    $"Limited 5★ Pity: {playerData.LimitedPity5}/{LimitedBanner.HardPity5}");

                Console.WriteLine(
                    $"Limited 4★ Pity: {playerData.LimitedPity4}/{LimitedBanner.HardPity4}");
            }
            else
            {
                Console.WriteLine(
                    $"Standard 5★ Pity: {playerData.StandardPity5}/{StandardBanner.HardPity5}");

                Console.WriteLine(
                    $"Standard 4★ Pity: {playerData.StandardPity4}/{StandardBanner.HardPity4}");
            }
        }
    }
}