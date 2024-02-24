using Microsoft.EntityFrameworkCore;
using RandomLoadoutGenerator.Database;
using RandomLoadoutGenerator.Models;

namespace RandomLoadoutGenerator;

/// <summary>
/// A class with methods meant for randomizing Team Fortress 2 classes. weapons and slots.
/// </summary>
public sealed class Generator
{
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
#if NET8_0
        _loadoutCombinations = [.. database.LoadoutCombinations];
        _weapons = [.. database.Weapons
                       .Include(w => w.ReskinGroup)
                       .Include(w => w.LoadoutCombos)
                       .AsSplitQuery()];
#else
        _loadoutCombinations = database.LoadoutCombinations.ToList();
        _weapons = database.Weapons
            .Include(w => w.ReskinGroup)
            .Include(w => w.LoadoutCombos)
            .AsSplitQuery()
            .ToList();
#endif
    }

    /// <summary>
    /// Creates a new <see cref="Generator"/> instance and attempts to populate its fields with records from the passed <see cref="DatabaseContext"/> instance.
    /// </summary>
    /// <param name="database">DI of <see cref="DatabaseContext"/>. Instance will query all tables from this context.</param>
    public Generator(DatabaseContext database)
    {
        //Despite this constructor not technically using the DB file
        //unit-testing requires that it is there
        if (!DatabaseFile.Exists())
            DatabaseFile.Unpack();

#if NET8_0
        //_reskinGroups = database.ReskinGroups.ToList();
        _loadoutCombinations = [.. database.LoadoutCombinations];
        _weapons = [.. database.Weapons
                       .Include(w => w.ReskinGroup)
                       .Include(w => w.LoadoutCombos)
                       .AsSplitQuery()];
#else
        _loadoutCombinations = database.LoadoutCombinations.ToList();
        _weapons = database.Weapons
            .Include(w => w.ReskinGroup)
            .Include(w => w.LoadoutCombos)
            .AsSplitQuery()
            .ToList();
#endif
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
        if (targetClass != TFClass.Spy && targetSlot == TFSlot.Sapper)
            throw new ArgumentException($"{targetClass} and {targetSlot} is not a valid combination.");

        //Get the exact needed TFLoadoutCombo instance because comparing objects in C# can be wonky
        var targetLoadoutCombo = (
            from loadout in _loadoutCombinations
            where loadout.Class == targetClass && loadout.Slot == targetSlot
            select loadout
        ).First();

        //Get weapons valid for the requested loadout slot & class
        var validWeapons =
            from weapon in _weapons
            where weapon.LoadoutCombos!.Contains(targetLoadoutCombo) && weapon.IsEnabled
            select weapon;

        //TODO: throw up on the carpet when all the weapons were disabled, do NOT tell mom

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
    /// Enables the specified weapons to be in the pool of possible outcomes for this instance.
    /// <para>
    /// If no weapon is found with the specified ID, <see cref="ArgumentException"/> will be thrown.
    /// Take a look at the bottom of the <c>README.md</c> to see weapon IDs.
    /// Attempting to enable an already enabled weapon does nothing.
    /// </para>
    /// <para>
    /// All weapons are enabled by default. Keep in mind that disabled weapons will be disabled only for that instance. A new <see cref="Generator"/> instance will have all weapons re-enabled.
    /// </para>
    /// </summary>
    /// <param name="weaponIds">An array of ID's of weapons to enable.</param>
    /// <exception cref="ArgumentException"/>
    public void EnableWeapons(params int[] weaponIds)
    {
        foreach (var id in weaponIds)
        {
            var weapon = _weapons.Find(w => w.ID == id) ?? throw new ArgumentException($"No weapon with ID: {id} was found.");
            weapon.IsEnabled = true;
        }
    }

    /// <summary>
    /// Returns all weapons enabled for this instance.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all weapons currently enabled for this instance.</returns>
    public IEnumerable<Weapon> GetEnabledWeapons()
    {
        return from weapon in _weapons
               where weapon.IsEnabled
               select weapon;
    }

    /// <summary>
    /// Returns all enabled weapons usable by the specified Team Fortress 2 class. This includes mutli-class weapons.
    /// </summary>
    /// <param name="targetClass">The target TF2 class.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all weapons usable by specified class and currently enabled for this instance.</returns>
    public IEnumerable<Weapon> GetEnabledWeapons(TFClass targetClass)
    {
        var weaponLists = from loadoutCombo in _loadoutCombinations
                          where loadoutCombo.Class == targetClass
                          select loadoutCombo.Weapons;

        var result = new List<Weapon>();
        foreach (var weaponList in weaponLists)
        {
            result.AddRange(weaponList.Where(weapon => weapon.IsEnabled));
        }
        return result;
    }

    /// <summary>
    /// Returns all enabled weapons usable in the specified loadout slot. This includes weapons equippable in multiple slots for multiple classes.
    /// </summary>
    /// <param name="targetSlot">The target loadout slot.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all weapons equippable in the specified loadout and currently enabled for this instance.</returns>
    public IEnumerable<Weapon> GetEnabledWeapons(TFSlot targetSlot)
    {
        var weaponLists = from loadoutCombo in _loadoutCombinations
                          where loadoutCombo.Slot == targetSlot
                          select loadoutCombo.Weapons;

        var result = new List<Weapon>();
        foreach (var weaponList in weaponLists)
        {
            result.AddRange(weaponList.Where(weapon => weapon.IsEnabled));
        }
        return result;
    }

    /// <summary>
    /// Returns all enabled weapons usable in the specified loadout slot by the specified class.
    /// </summary>
    /// <param name="targetClass">The target TF2 class.</param>
    /// <param name="targetSlot">The target loadout slot.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all weapons equippable in the specified loadout by specified TF2 class and currently enabled for this instance.</returns>
    public IEnumerable<Weapon> GetEnabledWeapons(TFClass targetClass, TFSlot targetSlot)
    {
        if (targetClass != TFClass.Spy && targetSlot == TFSlot.Sapper)
            throw new ArgumentException($"{targetClass} and {targetSlot} is not a valid combination.");

        var weaponLists = from loadoutCombo in _loadoutCombinations
                          where loadoutCombo.Class == targetClass && loadoutCombo.Slot == targetSlot
                          select loadoutCombo.Weapons;

        var result = new List<Weapon>();
        foreach (var weaponList in weaponLists)
        {
            result.AddRange(weaponList.Where(weapon => weapon.IsEnabled));
        }
        return result;
    }

    /// <summary>
    /// Disables the specified weapons and pulls them out of the pool of possible outcomes.
    /// Makes it impossible to pull the specified weapons, until they're re-enabled.
    /// <para>
    /// If no weapon is found with the specified ID, <see cref="ArgumentException"/> will be thrown.
    /// Take a look at the bottom of the <c>README.md</c> to see weapon IDs.
    /// <b>TODO: Disabling last possible weapon - what should happen???</b>
    /// </para>
    /// <para>
    /// All weapons are enabled by default. Keep in mind that disabled weapons will be disabled only for that instance. A new <see cref="Generator"/> instance will have all weapons re-enabled.
    /// </para>
    /// </summary>
    /// <param name="weaponIds">An array of ID's of weapons to disable.</param>
    /// <exception cref="ArgumentException"/>
    public void DisableWeapons(params int[] weaponIds)
    {
        foreach (var id in weaponIds)
        {
            var weapon = _weapons.Find(w => w.ID == id) ?? throw new ArgumentException($"No weapon with ID: {id} was found.");
            weapon.IsEnabled = false;
        }
    }

    /// <summary>
    /// Returns all weapons disabled for this instance.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all weapons currently disabled for this instance.</returns>
    public IEnumerable<Weapon> GetDisabledWeapons()
    {
        return from weapon in _weapons
               where !weapon.IsEnabled
               select weapon;
    }

    /// <summary>
    /// Returns all disabled weapons usable by the specified Team Fortress 2 class. This includes mutli-class weapons.
    /// </summary>
    /// <param name="targetClass">The target TF2 class.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all weapons usable by specified class and currently disabled for this instance.</returns>
    public IEnumerable<Weapon> GetDisabledWeapons(TFClass targetClass)
    {
        var weaponLists = from loadoutCombo in _loadoutCombinations
                          where loadoutCombo.Class == targetClass
                          select loadoutCombo.Weapons;

        var result = new List<Weapon>();
        foreach (var weaponList in weaponLists)
        {
            result.AddRange(weaponList.Where(weapon => !weapon.IsEnabled));
        }
        return result;
    }

    /// <summary>
    /// Returns all disabled weapons usable in the specified loadout slot. This includes weapons equippable in multiple slots for multiple classes.
    /// </summary>
    /// <param name="targetSlot">The target loadout slot.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all weapons equippable in the specified loadout and currently disabled for this instance.</returns>
    public IEnumerable<Weapon> GetDisabledWeapons(TFSlot targetSlot)
    {
        var weaponLists = from loadoutCombo in _loadoutCombinations
                          where loadoutCombo.Slot == targetSlot
                          select loadoutCombo.Weapons;

        var result = new List<Weapon>();
        foreach (var weaponList in weaponLists)
        {
            result.AddRange(weaponList.Where(weapon => !weapon.IsEnabled));
        }
        return result;
    }

    /// <summary>
    /// Returns all disabled weapons usable in the specified loadout slot by the specified class.
    /// </summary>
    /// <param name="targetClass">The target TF2 class.</param>
    /// <param name="targetSlot">The target loadout slot.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all weapons equippable in the specified loadout by specified TF2 class and currently disabled for this instance.</returns>
    public IEnumerable<Weapon> GetDisabledWeapons(TFClass targetClass, TFSlot targetSlot)
    {
        if (targetClass != TFClass.Spy && targetSlot == TFSlot.Sapper)
            throw new ArgumentException($"{targetClass} and {targetSlot} is not a valid combination.");

        var weaponLists = from loadoutCombo in _loadoutCombinations
                          where loadoutCombo.Class == targetClass && loadoutCombo.Slot == targetSlot
                          select loadoutCombo.Weapons;

        var result = new List<Weapon>();
        foreach (var weaponList in weaponLists)
        {
            result.AddRange(weaponList.Where(weapon => !weapon.IsEnabled));
        }
        return result;
    }

    /// <summary>
    /// Returns all weapons in the instance.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all available weapons.</returns>
    public IEnumerable<Weapon> GetAllWeapons() => _weapons;

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
