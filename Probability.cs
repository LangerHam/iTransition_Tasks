using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    static class Probability
    {
        public static double WinRate(Die d1, Die d2) =>
            d1.Faces.SelectMany(f1 => d2.Faces.Select(f2 => f1 > f2 ? 1.0 : 0.0)).Average();

        public static void ShowTable(List<Die> dice)
        {
            Console.WriteLine("\nWin Probability Table (row beats column):");

            var diceNames = dice.Select(d => d.ToString()).ToList();
            var maxDiceNameLength = diceNames.Max(name => name.Length);
            var headerWidth = Math.Max(maxDiceNameLength, 4); 
            var colWidth = 7; 

            Console.Write("".PadRight(headerWidth + 3)); 
            for (int i = 0; i < dice.Count; i++)
            {
                Console.Write($"Die {i}".PadRight(colWidth));
            }
            Console.WriteLine();

            Console.Write("".PadRight(headerWidth + 3, '-'));
            for (int i = 0; i < dice.Count; i++)
            {
                Console.Write("".PadRight(colWidth, '-'));
            }
            Console.WriteLine();

            for (int i = 0; i < dice.Count; i++)
            {
                Console.Write($"{diceNames[i]}".PadRight(headerWidth + 3));
                for (int j = 0; j < dice.Count; j++)
                {
                    if (i == j)
                        Console.Write("-".PadRight(colWidth));
                    else
                        Console.Write($"{WinRate(dice[i], dice[j]):F3}".PadRight(colWidth));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
