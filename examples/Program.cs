using RandomLoadoutGenerator;
using RandomLoadoutGenerator.Models;

namespace ExampleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Instantiate a Generator object
            var generator = new Generator();

            while (true)
            {
                //Get a random class
                var randomClass = Generator.RandomizeClass();

                //Randomize each loadout slot
                Weapon primary = generator.RandomizeWeapon(randomClass, TFSlot.Primary);
                Weapon secondary = generator.RandomizeWeapon(randomClass, TFSlot.Secondary);
                Weapon melee = generator.RandomizeWeapon(randomClass, TFSlot.Melee);

                //If the random class is a Spy, also randomize a sapper
                Weapon? sapper = randomClass.Equals(TFClass.Spy) ? generator.RandomizeWeapon(TFClass.Spy, TFSlot.Sapper) : null;

                string loadout = $"{primary.Name}, {secondary.Name}, {melee.Name}";
                if (sapper is not null)
                    loadout += $", {sapper.Name}";

                Console.WriteLine($"Your class is {randomClass} and your loadout is:\n {loadout}");
                Console.ReadLine();
            }
        }
    }
}
