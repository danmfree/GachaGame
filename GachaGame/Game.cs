using System.Linq;
using GachaGame.Gacha;
using GachaGame.Models;
using GachaGame.Systems;
using GachaGame.UI;


namespace GachaGame
{
    public class Game
    {
        private readonly SaveSystem saveSystem;

        private readonly PlayerData playerData;

        private readonly CurrencySystem currencySystem;
        private readonly RewardSystem rewardSystem;
        private readonly Banner banner;

        private readonly CollectionSystem collectionSystem;
        private readonly HistorySystem historySystem;

        public Game()
        {
            saveSystem = new SaveSystem();

            playerData = saveSystem.Load();

            currencySystem = new CurrencySystem(playerData);

            rewardSystem = new RewardSystem(
                currencySystem,
                playerData
            );

            banner = new Banner(playerData);


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

                Console.WriteLine("1. Single Pull");
                Console.WriteLine("2. Pull x10");
                Console.WriteLine("3. Character Collection");
                Console.WriteLine("4. Pull History");
                Console.WriteLine("5. Claim Daily Reward");
                Console.WriteLine("6. Play Moon Trial");
                Console.WriteLine("7. Reset Save");
                Console.WriteLine("8. Exit");

                Console.Write("\nChoice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        DoPull(1);
                        break;

                    case "2":
                        DoPull(10);
                        break;

                    case "3":
                        ShowCollection();
                        break;

                    case "4":
                        ShowHistory();
                        break;

                    case "5":
                        rewardSystem.ClaimDailyReward();
                        saveSystem.Save(playerData);
                        Console.ReadLine();
                        break;

                    case "6":
                        rewardSystem.PlayMoonTrial();
                        saveSystem.Save(playerData);
                        Console.ReadLine();
                        break;

                    case "7":
                        ResetGame();
                        break;

                    case "8":
                        saveSystem.Save(playerData);
                        running = false;
                        break;
                }
            }

            Console.WriteLine("Thanks for playing!");
        }

        private void DoPull(int amount)
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
                results.Add(banner.Pull());
            }

            Console.Clear();

            LoadingAnimation.ShowLoadingBar(results);

            foreach (var pulled in results)
            {
                historySystem.AddPull(pulled);

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
            Console.WriteLine($"Pity to next 5-star: {playerData.Pity5}/{Banner.HardPity5}");
            Console.WriteLine($"Pity to next 4-star: {playerData.Pity4}/{Banner.HardPity4}");

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

        private void ShowHistory()
        {
            int page = 0;

            while (true)
            {
                Console.Clear();

                int totalPages = Math.Max(1, (historySystem.History.Count + 9) / 10);

                Console.WriteLine("===== Pull History =====");
                Console.WriteLine($"Page {page + 1}/{totalPages}");
                Console.WriteLine();

                var pageEntries = historySystem.History
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
                    Console.ResetColor();

                    Console.Write($"{h.Character.Name,-35}");

                    if (h.Pity.HasValue)
                        Console.Write($" Pity {h.Pity.Value}");

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

            Console.WriteLine("Are you sure you want to reset your account?");
            Console.WriteLine("Type YES to confirm:");

            string input = Console.ReadLine();

            if (input == "YES")
            {
                saveSystem.ResetSave();

                Console.WriteLine();
                Console.WriteLine("Account reset successfully!");
                Console.WriteLine("Please restart the game.");

                Console.WriteLine();
                Console.Write("Press Enter...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Reset cancelled.");
                Console.ReadLine();
            }
        }
    }
}