using System.Linq;
using GachaGame.Gacha;
using GachaGame.Models;
using GachaGame.Systems;
using GachaGame.UI;


namespace GachaGame
{
    public class Game
    {
        private SaveSystem saveSystem;

        private PlayerData playerData;

        private CurrencySystem currencySystem;
        private RewardSystem rewardSystem;

        private LimitedBanner limitedBanner;
        private StandardBanner standardBanner;

        private CollectionSystem collectionSystem;
        private HistorySystem historySystem;

        public Game()
        {
            saveSystem = new SaveSystem();

            playerData = saveSystem.Load();

            currencySystem = new CurrencySystem(playerData);

            rewardSystem = new RewardSystem(
                currencySystem,
                playerData
            );

            limitedBanner = new LimitedBanner(playerData);
            standardBanner = new StandardBanner(playerData);

            collectionSystem = new CollectionSystem(playerData);
            historySystem = new HistorySystem(playerData);
        }

        private const int SinglePullCost = 160;
        private const int TenPullCost = 1600;

        public void Run()
        {
            Console.Title = "Silly Gacha";

            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine($"Moon Tears: {currencySystem.Amount}");
                Console.WriteLine();

                Console.WriteLine("1. Banners");
                Console.WriteLine("2. Character Collection");
                Console.WriteLine("3. Claim Daily Reward");
                Console.WriteLine("4. Play Moon Trial");
                Console.WriteLine("5. Reset Account");
                Console.WriteLine("6. Exit");

                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ShowBannerMenu();
                        break;

                    case "2":
                        ShowCollection();
                        break;

                    case "3":
                        rewardSystem.ClaimDailyReward();
                        saveSystem.Save(playerData);
                        Console.ReadLine();
                        break;

                    case "4":
                        rewardSystem.PlayMoonTrial();
                        saveSystem.Save(playerData);
                        Console.ReadLine();
                        break;

                    case "5":
                        ResetGame();
                        break;

                    case "6":
                        saveSystem.Save(playerData);
                        running = false;
                        break;
                }
            }

            Console.WriteLine("Thanks for playing!");
        }

        private void DoPull(int amount, bool limitedBannerPull)
        {
            int cost = amount == 1 ? SinglePullCost : TenPullCost;

            if (!currencySystem.Spend(cost))
            {
                Console.Clear();
                Console.WriteLine("Not enough Moon Tears!");
                Console.WriteLine();

                Console.WriteLine($"Moon Tears: {currencySystem.Amount}");

                Console.WriteLine();
                Console.Write("Press Enter to return...");
                Console.ReadLine();
                //saveSystem.Save(playerData);

                return;
            }

            var results = new List<Character>();
            var duplicateMessages = new List<string>();

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

            Console.Clear();

            LoadingAnimation.ShowLoadingBar(results);

            foreach (var pulled in results)
            {
                historySystem.AddPull(pulled, limitedBannerPull);

                int duplicateReward = collectionSystem.AddCharacter(pulled);

                if (duplicateReward > 0)
                {
                    currencySystem.Add(duplicateReward);

                    string rewardMessage = "";

                    if (pulled.Rarity == Rarity.FourStar)
                    {
                        rewardMessage =
                            $"{pulled.Name} is already C6!\n" +
                            $"Received {duplicateReward} Moon Tears.";
                    }
                    else if (pulled.Rarity == Rarity.FiveStar)
                    {
                        rewardMessage =
                            $"{pulled.Name} is already C6!\n" +
                            $"Received {duplicateReward} Moon Tears.";
                    }

                    duplicateMessages.Add(rewardMessage);
                }
            }

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

            if (duplicateMessages.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("=== Duplicate Rewards ===");
                Console.WriteLine();

                foreach (var message in duplicateMessages)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(message);
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            if (limitedBannerPull)
            {
                Console.WriteLine($"Next Limited 5★ Pity: {playerData.LimitedPity5}/{LimitedBanner.HardPity5}");
                Console.WriteLine($"Next Limited 4★ Pity: {playerData.LimitedPity4}/{LimitedBanner.HardPity4}");
            }
            else
            {
                Console.WriteLine($"Next Standard 5★ Pity: {playerData.StandardPity5}/{StandardBanner.HardPity5}");
                Console.WriteLine($"Next Standard 4★ Pity: {playerData.StandardPity4}/{StandardBanner.HardPity4}");
            }

            Console.WriteLine();
            Console.Write("Press Enter to return...");
            Console.ReadLine();
            saveSystem.Save(playerData);
        }

        private void ShowCollection()
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
            Console.Write("Press Enter to return...");
            Console.ReadLine();
        }

        private void ShowBannerMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("========== BANNERS ==========");
                Console.WriteLine();
                Console.WriteLine("1. Event Character Banner");
                Console.WriteLine("2. Standard Banner");
                Console.WriteLine("3. Back");
                Console.WriteLine();

                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ShowEventBannerMenu();
                        break;

                    case "2":
                        ShowStandardBannerMenu();
                        break;

                    case "3":
                        return;
                }
            }
        }

        private void ShowEventBannerMenu()
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
                Console.WriteLine("4. Back");
                Console.WriteLine();

                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        DoPull(1, true);
                        break;

                    case "2":
                        DoPull(10, true);
                        break;

                    case "3":
                        ShowHistory(true);
                        break;

                    case "4":
                        return;
                }
            }
        }

        private void ShowStandardBannerMenu()
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
                Console.WriteLine("4. Back");
                Console.WriteLine();

                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        DoPull(1, false);
                        break;

                    case "2":
                        DoPull(10, false);
                        break;

                    case "3":
                        ShowHistory(false);
                        break;

                    case "4":
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
                Console.WriteLine("[A] Previous   [D] Next   [Q] Back");

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

                    case ConsoleKey.Q:
                        return;
                }
            }
        }

        // Helper 

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

        private void ResetGame()
        {
            Console.Clear();

            Console.WriteLine("Are you sure you want to reset?");
            Console.Write("Type YES: ");

            string input = Console.ReadLine();

            if (input == "YES")
            {
                saveSystem.ResetSave();

                ReloadGameData();

                Console.WriteLine();
                Console.WriteLine("Account reset successfully!");
                Console.WriteLine($"Moon Tears: {currencySystem.Amount}");
            }
            else
            {
                Console.WriteLine("Reset cancelled.");
            }

            Console.WriteLine();
            Console.Write("Press Enter...");
            Console.ReadLine();
        }

        private void ReloadGameData()
        {
            playerData = saveSystem.Load();

            currencySystem = new CurrencySystem(playerData);

            rewardSystem = new RewardSystem(
                currencySystem,
                playerData
            );

            limitedBanner = new LimitedBanner(playerData);
            standardBanner = new StandardBanner(playerData);

            collectionSystem = new CollectionSystem(playerData);

            historySystem = new HistorySystem(playerData);
        }
    }
}