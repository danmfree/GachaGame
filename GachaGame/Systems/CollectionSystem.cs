using GachaGame.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GachaGame.Systems
{
    public class CollectionSystem
    {
        private readonly List<OwnedCharacter> collection = new();

        public IReadOnlyList<OwnedCharacter> Collection => collection;

        public void AddCharacter(Character character)
        {
            // Don't store 3-star characters
            if (character.Rarity == Rarity.ThreeStar)
                return;

            var ownedCharacter = collection.FirstOrDefault(c => c.Character.Name == character.Name);

            if (ownedCharacter == null)
            {
                collection.Add(new OwnedCharacter(character));
            }
            else if (ownedCharacter.Copies < 7)
            {
                ownedCharacter.Copies++;
            }
        }
    }
}
