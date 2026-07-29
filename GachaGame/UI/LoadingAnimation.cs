using GachaGame.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GachaGame.UI
{
    public static class LoadingAnimation
    {
        public static void ShowLoadingBar(List<Character> results)
        {
            Rarity highest = results.Max(c => c.Rarity);

            Console.WriteLine("Summoning...");

            for (int i = 0; i <= 100; i += 2)
            {
                Console.ForegroundColor = ConsoleColor.White;

                if (highest == Rarity.ThreeStar)
                {
                    if (i >= 40)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;

                    if (i >= 70)
                        Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (highest == Rarity.FourStar)
                {
                    if (i >= 25)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;

                    if (i >= 50)
                        Console.ForegroundColor = ConsoleColor.Blue;

                    if (i >= 70)
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;

                    if (i >= 90)
                        Console.ForegroundColor = ConsoleColor.Magenta;
                }
                else // Five Star
                {
                    if (i >= 15)
                        Console.ForegroundColor = ConsoleColor.DarkBlue;

                    if (i >= 35)
                        Console.ForegroundColor = ConsoleColor.Blue;

                    if (i >= 55)
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;

                    if (i >= 70)
                        Console.ForegroundColor = ConsoleColor.Magenta;

                    if (i >= 85)
                        Console.ForegroundColor = ConsoleColor.DarkYellow;

                    if (i >= 95)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                }

                DrawBar(i);
                Thread.Sleep(15);
            }

            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void DrawBar(int percent)
        {
            int width = 40;
            int filled = width * percent / 100;

            string bar = new string('#', filled) +
                         new string('-', width - filled);

            Console.Write($"\r[{bar}] {percent,3}%");
        }
    }
}
