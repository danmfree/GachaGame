using GachaGame.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GachaGame.UI
{
    public static class ConsoleRenderer
    {
        public static void PrintCharacter(Character character)
        {
            string stars = new string('*', (int)character.Rarity);

            Console.ForegroundColor = GetRarityColor(character.Rarity);

            string pityText = "";

            if (character.Rarity == Rarity.FiveStar)
                pityText = $" (Pity {character.PulledAtPity5})";
            else if (character.Rarity == Rarity.FourStar)
                pityText = $" (Pity {character.PulledAtPity4})";

            Console.WriteLine($"  [{stars}] {character.Name}{pityText}");

            Console.ResetColor();
        }

        public static void PrintCollectionCharacter(OwnedCharacter ownedCharacter)
        {
            string stars = new string('*', (int)ownedCharacter.Character.Rarity);

            Console.ForegroundColor = GetRarityColor(ownedCharacter.Character.Rarity);

            Console.WriteLine(
                $"  [{stars}] {ownedCharacter.Character.Name}   C{ownedCharacter.Copies - 1}");

            Console.ResetColor();
        }

        public static void ShowSummary(List<Character> results, int amount)
        {
            Console.WriteLine($"=== {amount}-Pull Summary ===");
            Console.WriteLine();

            foreach (var character in results.OrderByDescending(c => c.Rarity))
                PrintCharacter(character);

            int fiveStars = results.Count(c => c.Rarity == Rarity.FiveStar);
            int fourStars = results.Count(c => c.Rarity == Rarity.FourStar);
            int threeStars = results.Count(c => c.Rarity == Rarity.ThreeStar);

            Console.WriteLine();
            Console.WriteLine(
                $"5-star: {fiveStars}   4-star: {fourStars}   3-star: {threeStars}");
        }

        public static ConsoleColor GetRarityColor(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.FiveStar => ConsoleColor.Yellow,
                Rarity.FourStar => ConsoleColor.Magenta,
                _ => ConsoleColor.Gray
            };
        }
    }
}
