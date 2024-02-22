using Microsoft.EntityFrameworkCore;
using RandomLoadoutGenerator.Database;
using RandomLoadoutGenerator.Models;

namespace RandomLoadoutGenerator;

/// <summary>
/// A class with methods meant for randomizing Team Fortress 2 classes and weapons.
/// </summary>
public sealed class Generator
{
    //TODO
    //readonly List<ReskinGroup> _reskinGroups;
    readonly List<LoadoutCombination> _loadoutCombinations;
    readonly List<Weapon> _weapons;

    /// <summary>
    /// Creates a new <see cref="Generator"/> instance and attempts to populate its fields with records from the bundled SQLite database.<br/>
    /// </summary>
    public Generator()
    {
        if (!DatabaseFile.Exists())
            DatabaseFile.Unpack();

        using var database = new DatabaseContext();
        //_reskinGroups = database.ReskinGroups.ToList();
        _loadoutCombinations = database.LoadoutCombinations.ToList();
        _weapons = database.Weapons
            .Include(w => w.ReskinGroup)
            .Include(w => w.LoadoutCombos)
            .AsSplitQuery()
            .ToList();
    }

    /// <summary>
    /// Creates a new <see cref="Generator"/> instance and attempts to populate its fields with records from the passed <see cref="DatabaseContext"/> instance.
    /// </summary>
    /// <param name="database">DI of <see cref="DatabaseContext"/>. Instance will query all tables from this context.</param>
    public Generator(DatabaseContext database)
    {
        if (!DatabaseFile.Exists())
            DatabaseFile.Unpack();

        //_reskinGroups = database.ReskinGroups.ToList();
        _loadoutCombinations = database.LoadoutCombinations.ToList();
        _weapons = database.Weapons
            .Include(w => w.ReskinGroup)
            .Include(w => w.LoadoutCombos)
            .AsSplitQuery()
            .ToList();
    }

    /// <summary>
    /// Randomizes a single weapon for the given class and the given slot.<br/>
    /// <see cref="ArgumentException"/> will be thrown if the passed class/slot combo is not valid, such as <see cref="TFClass.Scout"/> with <see cref="TFSlot.Sapper"/>.
    /// </summary>
    /// <param name="targetClass">The <see cref="TFClass"/> for which the weapon will be randomized.</param>
    /// <param name="targetSlot">The <see cref="TFSlot"/> for which the weapon will be randomized.</param>
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

    /// <summary>
    /// Returns one of the Team Fortress 2 playable classes, based on random chance.<br/>
    /// Be careful when randomizing both <see cref="TFSlot"/> and <see cref="TFClass"/> and passing it to <see cref="RandomizeWeapon(TFClass, TFSlot, bool)"/>, you might end up with an invalid combination.
    /// </summary>
    /// <returns>A randomly selected value from <see cref="TFClass"/> enum.</returns>
    public static TFClass RandomizeClass()
    {
        var enumAsArray = Enum.GetValues(typeof(TFClass));
        var randomIndex = Random.Shared.Next(0, enumAsArray.Length);
        return (TFClass)enumAsArray.GetValue(randomIndex)!;
    }

    /// <summary>
    /// Returns one of the Team Fortress 2 loadout slots, based on random chance. Setting <i>allowSapper</i> to <c>true</c> will allow a <see cref="TFSlot.Sapper"/> to be a possible outcome. <br/>
    /// Be careful when randomizing both <see cref="TFSlot"/> and <see cref="TFClass"/> and passing it to <see cref="RandomizeWeapon(TFClass, TFSlot, bool)"/>, you might end up with an invalid combination.
    /// </summary>
    /// <param name="allowSapper">If <c>true</c>, <see cref="TFSlot.Sapper"/> will be one of the possible outcomes. Otherwise, it will be never returned.</param>
    /// <returns>A randomly selected value from <see cref="TFSlot"/> enum.</returns>
    public static TFSlot RandomizeSlot(bool allowSapper = false)
    {
        var possibleOutcomes = new List<TFSlot>();
        foreach (TFSlot slot in Enum.GetValues(typeof(TFSlot)))
        {
            if (!slot.Equals(TFSlot.Sapper) || allowSapper)
                possibleOutcomes.Add(slot);
        }
        return possibleOutcomes[Random.Shared.Next(0, possibleOutcomes.Count)];
    }
}
