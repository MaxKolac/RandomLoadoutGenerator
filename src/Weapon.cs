namespace RandomLoadoutGenerator;

/// <summary>
/// A single weapon available in Team Fortress 2.
/// </summary>
public class Weapon
{
    private string _weaponImageUri = string.Empty;
    public const string ImageURIPrefix = @"imgs\weapons\";
    public const string ImageURISuffix = ".png";

    public int ID { get; set; }
    /// <summary>
    /// Name of the weapon. Shouldn't contain the "The" prefix.
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// URI to the image of associated weapon. It contains a constant prefix of 'Data\Weapons\'.
    /// </summary>
    [Microsoft.EntityFrameworkCore.BackingField(nameof(_weaponImageUri))]
    public string ImageURI
    {
        get { return ImageURIPrefix + _weaponImageUri + ImageURISuffix; }
        set { _weaponImageUri = value; }
    }
    /// <summary>
    /// Whether or not is the weapon is a default choice in their respective class and slot.
    /// Default is <c>false</c>.
    /// </summary>
    public bool IsStock { get; set; } = false;

    //One-to-many
    public int? ReskinGroupID { get; set; }
    public ReskinGroup? ReskinGroup { get; set; }

    //Many-to-many 
    public IList<LoadoutCombination>? LoadoutCombos { get; set; }

    public override string ToString()
    {
        return $"{{Weapon: {ID}, {Name}}}";
    }
}
