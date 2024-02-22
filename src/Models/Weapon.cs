namespace RandomLoadoutGenerator.Models;

/// <summary>
/// A single weapon available in Team Fortress 2.
/// </summary>
public class Weapon
{
    private string _weaponImageUri = string.Empty;
    const string ImageURIPrefix = @"imgs\weapons\";
    const string ImageURISuffix = ".png";

    /// <summary>
    /// Unique ID number.
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// Name of the weapon.
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// URI to the image of associated weapon. <b>NOT IMPLEMENTED YET</b>
    /// </summary>
    [Microsoft.EntityFrameworkCore.BackingField(nameof(_weaponImageUri))]
    public string ImageURI
    {
        get { return ImageURIPrefix + _weaponImageUri + ImageURISuffix; }
        set { _weaponImageUri = value; }
    }
    /// <summary>
    /// Whether or not is the weapon is a default choice in their respective Team Fortress 2 class and slot.
    /// Default is <c>false</c>.
    /// </summary>
    public bool IsStock { get; set; } = false;
    /// <summary>
    /// If a weapon is enabled, it means it's in the pool of possible outcomes. Disabling a weapon removes it from the pool and makes it impossible to randomly get this weapon until it's re-enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// The ID of the <see cref="Models.ReskinGroup"/> to which this weapon belongs. <c>null</c> means that this weapon does not have any reskins.
    /// </summary>
    public int? ReskinGroupID { get; set; }
    /// <summary>
    /// Accessor member - one-to-many relation. Use this to access <see cref="Models.ReskinGroup"/> that this weapon belongs to, if it has any.
    /// </summary>
    public ReskinGroup? ReskinGroup { get; set; }

    //Many-to-many 
    /// <summary>
    /// Accessor member - many-to-many relation. Use this to access all the <see cref="LoadoutCombination"/>s this weapon is included in.
    /// </summary>
    public IList<LoadoutCombination>? LoadoutCombos { get; set; }
    
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{{Weapon: {ID}, {Name}}}";
    }
}
