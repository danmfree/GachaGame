using GachaGame.Gacha;
using System;
using System.Collections.Generic;
using System.Text;

namespace GachaGame.Models
{
    public class PullHistory
    {
        public Character Character { get; set; }
        public int PullNumber { get; set; }
        public int? Pity { get; set; }

        public PullHistory(Character character, int pullNumber, int? pity)
        {
            Character = character;
            PullNumber = pullNumber;
            Pity = pity;
        }
    }
}