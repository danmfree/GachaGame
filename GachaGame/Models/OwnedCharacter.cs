using System;
using System.Collections.Generic;
using System.Text;

namespace GachaGame.Models
{
    public class OwnedCharacter
    {
        public Character Character { get; set; }
        public int Copies { get; set; }
        public OwnedCharacter(Character character)
        {
            Character = character;
            Copies = 1;
        }
    }
}