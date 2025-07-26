using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace Task3
{
    static class Probability
    {
        public static double WinRate(Die d1, Die d2) =>
            d1.Faces.SelectMany(f1 => d2.Faces.Select(f2 => f1 > f2 ? 1.0 : 0.0)).Average();

        public static void ShowTable(List<Die> dice)
        {
            var header = new[] { "Die A \\ Die B" }
                     .Concat(Enumerable.Range(0, dice.Count).Select(i => $"D{i}"))
                     .ToArray();
            var table = new ConsoleTable(header);

            for (int i = 0; i < dice.Count; i++)
            {
                var row = new[] { $"D{i}" }
                          .Concat(Enumerable.Range(0, dice.Count)
                              .Select(j => i == j ? "-" : $"{Probability.WinRate(dice[i], dice[j]):P1}"))
                          .ToArray();
                table.AddRow(row);
            }

            Console.WriteLine("\nWinning probabilities (Die A > Die B)");
            table.Write();
            Console.WriteLine();
        }
    }
}
