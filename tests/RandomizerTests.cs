using RandomLoadoutGenerator;
using RandomLoadoutGenerator.Database;

namespace RandomLoadoutGeneratorTests;

public class RandomizerTests
{
    private static readonly RLGDatabase DbFixture = new(Microsoft.Data.Sqlite.SqliteOpenMode.ReadOnly);

    [Theory]
    [InlineData(TFClass.Scout, TFSlot.Sapper)]
    [InlineData(TFClass.Soldier, TFSlot.Sapper)]
    [InlineData(TFClass.Pyro, TFSlot.Sapper)]
    [InlineData(TFClass.Demoman, TFSlot.Sapper)]
    [InlineData(TFClass.Heavy, TFSlot.Sapper)]
    [InlineData(TFClass.Engineer, TFSlot.Sapper)]
    [InlineData(TFClass.Medic, TFSlot.Sapper)]
    [InlineData(TFClass.Sniper, TFSlot.Sapper)]
    public static void NonexistentClassAndSlotComboThrowsArgumentException(TFClass targetClass, TFSlot targetSlot)
    {
        var randomizer = new RandomLoadoutGenerator.RandomLoadoutGenerator(DbFixture);
        Assert.Throws<ArgumentException>(() =>
        {
            randomizer.RandomizeWeapon(targetClass, targetSlot);
        });
    }

    [Theory]
    [MemberData(nameof(GetAllExistingLoadoutCombos), MemberType = typeof(RandomizerTests))]
    public static void ReturnedWeaponIsOneOfTheExpectedOutcomes(LoadoutCombination loadoutCombo, int attempts)
    {
        var randomizer = new RandomLoadoutGenerator.RandomLoadoutGenerator(DbFixture);
        var expectedWeapons = from weapon in DbFixture.Weapons
                              where weapon.LoadoutCombos!.Contains(loadoutCombo)
                              select weapon;

        for (int i = 0; i < attempts; i++)
        {
            var returnedWeapon = randomizer.RandomizeWeapon(loadoutCombo.Class, loadoutCombo.Slot);
            //Assert.Contains(returnedWeapon, expectedWeapons); <- i've experimented with results of the test in the debugger, this assert just straight up blind
            Assert.True(expectedWeapons.Contains(returnedWeapon));
        }
    }

    public static IEnumerable<object[]> GetAllExistingLoadoutCombos()
    {
        foreach (var combo in DbFixture.LoadoutCombinations)
        {
            yield return new object[] { combo, 3 };
        }
    }
}
