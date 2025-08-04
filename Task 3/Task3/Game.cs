using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    class Game
    {
        private readonly List<Die> _dice;

        public Game(List<Die> dice) => _dice = dice;

        public void Play()
        {
            Console.WriteLine("Determining first move...");
            var computerFirst = FairRandom.Generate(2, "Guess my selection:") == 1;

            var (computerDie, userDie) = computerFirst
                ? (SelectComputerDie(), SelectUserDie("Choose your die:", null))
                : (SelectComputerDie(SelectUserDie("You go first, choose die:", null)), default);

            if (!computerFirst) userDie = _dice.First(d => d != computerDie);

            Console.WriteLine($"Computer: {computerDie}, You: {userDie}");

            Console.WriteLine("\nComputer rolling...");
            var compRoll = computerDie.Roll(FairRandom.Generate(6, "Add your number mod 6:"));
            Console.WriteLine($"Computer rolled: {compRoll}");

            Console.WriteLine("\nYour turn to roll...");
            var userRoll = userDie.Roll(FairRandom.Generate(6, "Add your number mod 6:"));
            Console.WriteLine($"You rolled: {userRoll}");

            Console.WriteLine(userRoll > compRoll ? $"You win! ({userRoll} > {compRoll})" :
                            compRoll > userRoll ? $"Computer wins! ({compRoll} > {userRoll})" :
                            $"Tie! ({userRoll} = {compRoll})");
        }

        private Die SelectComputerDie(Die? exclude = null) =>
            _dice.Where(d => d != exclude).OrderBy(_ => Crypt.SecureRandom(1000)).First();

        private Die SelectUserDie(string prompt, Die? exclude)
        {
            var available = _dice.Where(d => d != exclude).ToList();
            while (true)
            {
                Console.WriteLine(prompt);
                for (int i = 0; i < available.Count; i++)
                {
                    Console.WriteLine($"{i} - {available[i]}");
                }
                Console.WriteLine("X - exit");
                Console.WriteLine("? - help");
                Console.Write("Selection: ");

                var input = Console.ReadLine()?.Trim().ToUpper();
                switch (input)
                {
                    case "X":
                        Environment.Exit(0);
                        return null!;
                    case "?":
                        Probability.ShowTable(_dice);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue;
                    default:
                        if (int.TryParse(input, out var i) && i >= 0 && i < available.Count)
                            return available[i];
                        Console.WriteLine("Invalid choice! Please try again.");
                        continue;
                }
            }
        }
    }
}
