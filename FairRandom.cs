using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    static class FairRandom
    {
        public static int Generate(int max, string prompt)
        {
            var key = Crypt.SecureKey();
            var computerNum = Crypt.SecureRandom(max);

            Console.WriteLine($"I selected random value 0..{max - 1} (HMAC={Crypt.HMAC(key, computerNum.ToString())})");
            var userNum = GetChoice(max, prompt);

            Console.WriteLine($"My number: {computerNum} (KEY={Convert.ToHexString(key)})");
            var result = (computerNum + userNum) % max;
            Console.WriteLine($"Result: {computerNum} + {userNum} = {result} (mod {max})");
            return result;
        }

        private static int GetChoice(int max, string prompt)
        {
            Console.WriteLine(prompt);
            while (true)
            {
                for (int i = 0; i < max; i++)
                {
                    Console.WriteLine($"{i} - {i}");
                }
                Console.WriteLine("X - exit");
                Console.WriteLine("? - help");
                Console.Write("Your selection: ");

                var input = Console.ReadLine()?.Trim().ToUpper();
                switch (input)
                {
                    case "X":
                        Environment.Exit(0);
                        return 0;
                    case "?":
                        Console.WriteLine("Select a number from the available options to contribute to the fair random generation.");
                        continue;
                    default:
                        if (int.TryParse(input, out var n) && n >= 0 && n < max)
                            return n;
                        Console.WriteLine($"Invalid! Choose 0-{max - 1}, X, or ?:");
                        continue;
                }
            }
        }
    }
}

