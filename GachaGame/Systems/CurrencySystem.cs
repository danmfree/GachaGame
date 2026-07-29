using GachaGame.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GachaGame.Systems
{
    public class CurrencySystem
    {
        public string CurrencyName { get; } = "Moon Tears";

        private readonly PlayerData playerData;

        public CurrencySystem(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        public int Amount
        {
            get
            {
                return playerData.MoonTears;
            }
        }

        public bool Spend(int amount)
        {
            if (playerData.MoonTears < amount)
                return false;

            playerData.MoonTears -= amount;

            return true;
        }

        public void Add(int amount)
        {
            playerData.MoonTears += amount;
        }

        public bool CanAfford(int amount)
        {
            return playerData.MoonTears >= amount;
        }
    }
}
