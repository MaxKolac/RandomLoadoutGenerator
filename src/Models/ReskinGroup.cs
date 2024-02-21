namespace RandomLoadoutGenerator.Models;

/// <summary>
/// Describes a group of weapons to signify that despite being unique weapons, they all share exact same mechanics and work in similair way.<br/>
/// For example, Stock Pistol and Lugermorph share the same reskin group.
/// </summary>
public class ReskinGroup
{
    /// <summary>
    /// Unique ID number.
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// Name of the reskin group.
    /// </summary>
    public required string Name { get; set; }

    //One-to-many
    /// <summary>
    /// Accessor member - many-to-many relation. Use this to access <see cref="Weapon"/>s that belong to this <see cref="ReskinGroup"/>.
    /// </summary>
    public IList<Weapon>? Weapons { get; set; }
}
