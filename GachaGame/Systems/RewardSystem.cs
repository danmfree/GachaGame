using GachaGame.Models;
using GachaGame.Systems;
using System;

namespace GachaGame.Systems
{
    public class RewardSystem
    {
        private readonly CurrencySystem currencySystem;
        private readonly PlayerData playerData;

        private readonly Random random = new();

        public RewardSystem(CurrencySystem currencySystem, PlayerData playerData)
        {
            this.currencySystem = currencySystem;
            this.playerData = playerData;
        }

        private void ResetMoonTrialIfNewDay()
        {
            if (playerData.LastMoonTrialDate.Date != DateTime.Today)
            {
                playerData.MoonTrialAttempts = 0;
                playerData.LastMoonTrialDate = DateTime.Today;
            }
        }

        public void ClaimDailyReward()
        {
            DateTime today = DateTime.Today;

            if (playerData.LastDailyReward == today)
            {
                Console.WriteLine("You already claimed your Daily Reward!");
                return;
            }

            int reward = 160;

            currencySystem.Add(reward);

            playerData.LastDailyReward = today;

            Console.WriteLine("Daily Reward claimed!");
            Console.WriteLine($"+{reward} Moon Tears");
        }

        public void PlayMoonTrial()
        {
            ResetMoonTrialIfNewDay();

            if (playerData.MoonTrialAttempts >= 3)
            {
                Console.WriteLine("You have used all 3 Moon Trials today.");
                return;
            }


            playerData.MoonTrialAttempts++;


            Random random = new Random();

            int choice;

            while (true)
            {
                Console.Write("Choose a number (1-5): ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice >= 1 && choice <= 5)
                    {
                        break;
                    }
                }

                Console.WriteLine("Invalid choice. Please choose a number between 1 and 5.");
            }

            int answer = random.Next(1, 6);


            Console.WriteLine($"The answer was {answer}");


            if (choice == answer)
            {
                currencySystem.Add(1600);
                Console.WriteLine("Correct! You received 1600 Moon Tears.");
            }
            else
            {
                currencySystem.Add(800);
                Console.WriteLine("Wrong! You received 800 Moon Tears.");
            }


            Console.WriteLine(
                $"Moon Trials remaining: {3 - playerData.MoonTrialAttempts}");
        }
    }
}