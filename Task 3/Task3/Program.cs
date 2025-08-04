namespace Task3
{
class Program
{
    static void Main(string[] args)
    {
        try { new Game(DiceParser.Parse(args)).Play(); }
        catch (Exception ex) { Console.WriteLine(ex.Message); Environment.Exit(1); }
    }
}
}

