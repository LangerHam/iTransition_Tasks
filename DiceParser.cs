using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    static class DiceParser
    {
        public static List<Die> Parse(string[] args) =>
            args.Length < 3
                ? throw new ArgumentException($"Need ≥3 dice, got {args.Length}. Example: 2,2,4,4,9,9 6,8,1,1,8,6 7,5,3,7,5,3")
                : args.Select((arg, i) => ParseSingle(arg, i)).ToList();

        private static Die ParseSingle(string arg, int index) =>
            new Die(arg.Split(',').Select((s, j) =>
                int.TryParse(s.Trim(), out var val) ? val
                : throw new ArgumentException($"Die #{index + 1}: '{s}' not integer. Example: 2,2,4,4,9,9"))
            .ToArray() is var faces && faces.Length == 6 ? faces
            : throw new ArgumentException($"Die #{index + 1}: need 6 values, got {faces.Length}"));
    }
}
