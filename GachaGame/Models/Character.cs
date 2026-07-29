using System;
using System.Collections.Generic;
using System.Text;

namespace GachaGame.Models
{
    public class Character
    {
        public string Name { get; set; }
        public Rarity Rarity { get; set; }
        public int? PulledAtPity5 { get; set; }
        public int? PulledAtPity4 { get; set; }
        public Character(string name, Rarity rarity)
        {
            Name = name;
            Rarity = rarity;
        }
    }
}
