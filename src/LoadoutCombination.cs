namespace RandomLoadoutGenerator;

/// <summary>
/// Describes which class can equip a certain weapon, and in which slot.
/// </summary>
public class LoadoutCombination
{
    public int ID { get; set; }
    public required TFSlot Slot { get; set; }
    public required TFClass Class { get; set; }

    //Many-to-many
    public IList<Weapon>? Weapons { get; set; }
}
