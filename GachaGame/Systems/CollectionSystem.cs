using GachaGame.Models;
using System.Collections.Generic;
using System.Linq;

namespace GachaGame.Systems
{
    public class CollectionSystem
    {
        private readonly PlayerData playerData;


        public CollectionSystem(PlayerData playerData)
        {
            this.playerData = playerData;
        }


        public IReadOnlyList<OwnedCharacter> Collection => playerData.Characters;


        public int AddCharacter(Character character)
        {
            // Ignore 3-star characters
            if (character.Rarity == Rarity.ThreeStar)
                return 0;


            var ownedCharacter = playerData.Characters
                .FirstOrDefault(c => c.Character.Name == character.Name);


            // First time owning character
            if (ownedCharacter == null)
            {
                playerData.Characters.Add(new OwnedCharacter(character));
                return 0;
            }


            // Already C6, give duplicate reward
            if (ownedCharacter.Copies >= 7)
            {
                if (character.Rarity == Rarity.FourStar)
                    return 10 * 160;

                if (character.Rarity == Rarity.FiveStar)
                    return 40 * 160;
            }


            // Increase constellation
            ownedCharacter.Copies++;

            return 0;
        }
    }
}