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

        private GachaSystem gachaSystem;

        private PlayerData playerData;

        private CurrencySystem currencySystem;
        private RewardSystem rewardSystem;

        private LimitedBanner limitedBanner;
        private StandardBanner standardBanner;

        private CollectionSystem collectionSystem;
        private HistorySystem historySystem;

        private Menu menu;

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

            gachaSystem = new GachaSystem(
                currencySystem,
                limitedBanner,
                standardBanner,
                collectionSystem,
                historySystem,
                saveSystem,
                playerData
            );

            menu = new Menu(
                currencySystem,
                rewardSystem,
                limitedBanner,
                standardBanner,
                collectionSystem,
                historySystem,
                gachaSystem,
                playerData
            );
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

                Console.Write("\nChoice: ");

                string choice = menu.GetMenuChoice();

                switch (choice)
                {
                    case "1":
                        menu.ShowBannerMenu();
                        break;

                    case "2":
                        menu.ShowCollection();
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

                    case "ESC":
                        running = false;
                        break;
                }
            }

            Console.WriteLine("Thanks for playing!");
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


            gachaSystem = new GachaSystem(
                currencySystem,
                limitedBanner,
                standardBanner,
                collectionSystem,
                historySystem,
                saveSystem,
                playerData
            );


            menu = new Menu(
                currencySystem,
                rewardSystem,
                limitedBanner,
                standardBanner,
                collectionSystem,
                historySystem,
                gachaSystem,
                playerData
            );
        }
    }
}