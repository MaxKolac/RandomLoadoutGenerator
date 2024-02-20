using Microsoft.EntityFrameworkCore;
using RandomLoadoutGenerator.Database;

namespace RandomLoadoutGenerator;

public sealed class RandomLoadoutGenerator
{
    readonly List<ReskinGroup> _reskinGroups;
    readonly List<LoadoutCombination> _loadoutCombinations;
    readonly List<Weapon> _weapons;

    /// <summary>
    /// Creates a new <see cref="RandomLoadoutGenerator"/> object and populates its fields with data from the passed <see cref="DatabaseContext"/>.
    /// </summary>
    /// <param name="database">DI of <see cref="DatabaseContext"/>. Instance will query all tables from this context.</param>
    public RandomLoadoutGenerator(RLGDatabase database)
    {
        _reskinGroups = database.ReskinGroups.ToList();
        _loadoutCombinations = database.LoadoutCombinations.ToList();
        _weapons = database.Weapons
            .Include(w => w.ReskinGroup)
            .Include(w => w.LoadoutCombos)
            .AsSplitQuery()
            .ToList();
    }

    /// <summary>
    /// Randomizes a weapon for the given class and the given slot.<br/>
    /// <see cref="ArgumentException"/> will be thrown if the passed class/slot combo is not valid.
    /// </summary>
    /// <param name="targetClass">The <see cref="TFClass"/> for which the weapon will be randomized for.</param>
    /// <param name="targetSlot">The <see cref="TFSlot"/> for which the weapon will be randomized for.</param>
    /// <param name="treatReskinsAsOne">With this option enabled, all weapons which are considered reskins of each other will have only one of them put in the pool of possible weapons. In other words, enabling this option increases the chance of rolling for mechanically unique weapons.</param>
    /// <exception cref="ArgumentException"></exception>
    public Weapon RandomizeWeapon(TFClass targetClass, TFSlot targetSlot, bool treatReskinsAsOne = false)
    {
        //Get the exact needed TFLoadoutCombo instance because comparing objects in C# can be wonky
        var targetLoadoutCombo = (
            from loadout in _loadoutCombinations
            where loadout.Class == targetClass && loadout.Slot == targetSlot
            select loadout
        ).FirstOrDefault() ?? throw new ArgumentException($"{targetClass}, {targetSlot}");

        //Get weapons valid for the requested loadout slot & class
        var validWeapons =
            from weapon in _weapons
            where weapon.LoadoutCombos!.Contains(targetLoadoutCombo)
            select weapon;

        //With this option enabled, weapons are grouped by their ReskinGroup.
        //Each ReskinGroup adds only one of their weapons to the pool of possible weapons.
        //The weapons with null ReskinGroup are all added, since if this property is null, the weapon has no reskins and is always mechanically unique
        if (treatReskinsAsOne)
        {
            var weaponPool = new List<Weapon>();
            var reskins = (
                from weapon in validWeapons
                group weapon by weapon.ReskinGroup into reskinGroups
                select reskinGroups
                ).ToArray();

            foreach (var queryGroup in reskins)
            {
                if (queryGroup.Key is null)
                {
                    weaponPool.AddRange(queryGroup);
                }
                else
                {
                    weaponPool.Add(queryGroup.ToList()[Random.Shared.Next(queryGroup.Count())]);
                }
            }
            validWeapons = weaponPool.ToArray();
        }

        return validWeapons.ToArray()[Random.Shared.Next(validWeapons.Count())];
    }

    /// <returns>A randomly selected class from <see cref="TFClass"/>.</returns>
    public TFClass RandomizeClass()
    {
        var enumAsArray = Enum.GetValues(typeof(TFClass));
        var randomIndex = Random.Shared.Next(0, enumAsArray.Length);
        return (TFClass)enumAsArray.GetValue(randomIndex)!;
    }
}
