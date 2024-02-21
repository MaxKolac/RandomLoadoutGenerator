namespace RandomLoadoutGenerator;

/// <summary>
/// Enumerator describing all available loadout slots in Team Fortress 2.<br/>
/// In case of <see cref="TFClass.Spy"/>, his invisibility watches are considered <see cref="Secondary"/>. He's also the only class using the <see cref="Sapper"/> for his sappers.
/// </summary>
public enum TFSlot
{
    /// <summary>
    /// The primary slot. Scatterguns, rocket launcher, flamethrowers, demoknight booties - weapons that go to the upper-most slot on a loadout screen.
    /// </summary>
    Primary,
    /// <summary>
    /// The secondary slot. Pistols, banners, edibles - weapons that go to the middle slot on a loadout screen.<br/>
    /// The exception is <see cref="TFClass.Spy"/> for whom the <see cref="Secondary"/> slot is reserved for invisibility watches.
    /// </summary>
    Secondary,
    /// <summary>
    /// The tertiary slot. Shovels, alocohol beverages, fists, swords - weapons that go to the third slot counting from the top on a loadout screen.<br/>
    /// The exception is <see cref="TFClass.Spy"/> for whom the <see cref="Melee"/> slot is reserved for his knifes (instead of sappers).
    /// </summary>
    Melee,
    /// <summary>
    /// The additional fourth slot. The only weapons here are the titular Spy's sappers. Engineer's PDA is not in the weapon pool, since there's no unlockable weapons for its slot.
    /// </summary>
    Sapper
}

/// <summary>
/// Enumerator describing all playable classes in Team Fortress 2.
/// Their order and values follow the same rule as the class choice screen.
/// </summary>
public enum TFClass
{
    /// <summary>
    /// The Scout.
    /// </summary>
    Scout = 1,
    /// <summary>
    /// The Soldier.
    /// </summary>
    Soldier,
    /// <summary>
    /// The Pyro.
    /// </summary>
    Pyro,
    /// <summary>
    /// The Demoman.
    /// </summary>
    Demoman,
    /// <summary>
    /// The Heavy Weapons Guy.
    /// </summary>
    Heavy,
    /// <summary>
    /// The Engineer.
    /// </summary>
    Engineer,
    /// <summary>
    /// The Medic.
    /// </summary>
    Medic,
    /// <summary>
    /// The Sniper.
    /// </summary>
    Sniper,
    /// <summary>
    /// The Spy.
    /// </summary>
    Spy
}
