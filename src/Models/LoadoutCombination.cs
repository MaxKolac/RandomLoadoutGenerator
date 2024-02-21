namespace RandomLoadoutGenerator.Models;

/// <summary>
/// Describes which class can equip a certain weapon, and in which slot. 
/// </summary>
public class LoadoutCombination
{
    /// <summary>
    /// Unique ID number.
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// The <see cref="TFSlot"/> in which a <see cref="Weapon"/> can be equipped.
    /// </summary>
    public required TFSlot Slot { get; set; }
    /// <summary>
    /// The <see cref="TFClass"/> which can equip this <see cref="Weapon"/>.
    /// </summary>
    public required TFClass Class { get; set; }

    /// <summary>
    /// Accessor member - many-to-many relation. Use this to access <see cref="Weapon"/>s which can be equipped for this <see cref="LoadoutCombination"/>.
    /// </summary>
    public IList<Weapon>? Weapons { get; set; }
}
