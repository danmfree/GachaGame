using GachaGame.Gacha;
using GachaGame.Models;
using GachaGame.Systems;
using GachaGame.UI;

namespace ConsoleGacha
{
    public class Game
    {
        private readonly Banner banner = new();
        private readonly CollectionSystem collectionSystem = new();
        private readonly HistorySystem historySystem = new();

        public void Run()
        {
            Console.Title = "Console Gacha";

            bool running = true;

            while (running)
            {
                Console.Clear();

                Console.WriteLine("1. Single Pull");
                Console.WriteLine("2. Pull x10");
                Console.WriteLine("3. Character Collection");
                Console.WriteLine("4. Pull History");
                Console.WriteLine("5. Exit");

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
                        running = false;
                        break;
                }
            }

            Console.WriteLine("Thanks for playing!");
        }

        private void DoPull(int amount)
        {
            var results = new List<Character>();

            for (int i = 0; i < amount; i++)
            {
                results.Add(banner.Pull());
            }

            Console.Clear();

            LoadingAnimation.ShowLoadingBar(results);

            foreach (var pulled in results)
            {
                historySystem.AddPull(pulled);
                collectionSystem.AddCharacter(pulled);
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

            Console.WriteLine();
            Console.WriteLine($"Pity to next 5-star: {banner.Pity5}/{Banner.HardPity5}");
            Console.WriteLine($"Pity to next 4-star: {banner.Pity4}/{Banner.HardPity4}");

            Console.WriteLine();
            Console.Write("Press Enter to return...");
            Console.ReadLine();
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
    }
}