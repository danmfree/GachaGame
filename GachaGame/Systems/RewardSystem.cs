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
            int answer = random.Next(1, 6);

            Console.Write("Guess a number (1-5): ");

            int guess = int.Parse(Console.ReadLine());

            if (guess == answer)
            {
                currencySystem.Add(1600);

                Console.WriteLine("Correct!");
                Console.WriteLine("+1600 Moon Tears");
            }
            else
            {
                currencySystem.Add(800);

                Console.WriteLine($"Wrong! The answer was {answer}");
                Console.WriteLine("+800 Moon Tears");
            }
        }
    }
}