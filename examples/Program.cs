using RandomLoadoutGenerator;

namespace ExampleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var generator = new Generator(new());

            while (true)
            {
                var randomClass = Generator.RandomizeClass();
                var randomSlot = Generator.RandomizeSlot();
                var randomWeapon = generator.RandomizeWeapon(randomClass, randomSlot);

                Console.WriteLine($"{randomClass}, {randomSlot} -> {randomWeapon}");
                Thread.Sleep(1000);
            }
        }
    }
}
