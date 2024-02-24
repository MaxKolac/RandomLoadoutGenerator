namespace RandomLoadoutGenerator.Models;

/// <summary>
/// Describes which class can equip a certain weapon, and in which slot.<br/>
/// Comparing two <see cref="LoadoutCombination"/> only checks for <see cref="Slot"/> and <see cref="Class"/> values. Use <see cref="Equals(object?)"/> to include <see cref="ID"/> and <see cref="Weapons"/> values.
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

    ///<inheritdoc/>
    public static bool operator ==(LoadoutCombination left, LoadoutCombination right)
    {
        return left.Class == right.Class && left.Slot == right.Slot;
    }

    ///<inheritdoc/>
    public static bool operator !=(LoadoutCombination left, LoadoutCombination right)
    { 
        return !(left == right);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not LoadoutCombination)
            return false;
        
        var objAfterCast = obj as LoadoutCombination;
        return this == objAfterCast! && ID == objAfterCast!.ID && Weapons == objAfterCast.Weapons;
    }
    
    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
