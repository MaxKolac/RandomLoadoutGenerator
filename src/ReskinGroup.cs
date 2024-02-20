namespace RandomLoadoutGenerator;

/// <summary>
/// Describes a group of weapons to signify that despite being unique weapons, they all share exact same mechanics and work in similair way.
/// </summary>
public class ReskinGroup
{
    public int ID { get; set; }
    public required string Name { get; set; }

    //One-to-many
    public IList<Weapon>? Weapons { get; set; }
}
