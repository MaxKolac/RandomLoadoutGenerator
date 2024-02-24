using RandomLoadoutGenerator;
using RandomLoadoutGenerator.Database;
using RandomLoadoutGenerator.Models;

namespace RandomLoadoutGeneratorTests;

public class RandomizerTests
{
    static readonly DatabaseContext DbFixture = new(Microsoft.Data.Sqlite.SqliteOpenMode.ReadOnly);

    public static IEnumerable<object[]> GetAllExistingLoadoutCombos()
    {
        foreach (var combo in DbFixture.LoadoutCombinations)
        {
            yield return new object[] { combo, 5 };
        }
    }

    public static IEnumerable<object[]> GetAllInvalidLoadoutCombos()
    {
        yield return new object[] { TFClass.Scout, TFSlot.Sapper };
        yield return new object[] { TFClass.Soldier, TFSlot.Sapper };
        yield return new object[] { TFClass.Pyro, TFSlot.Sapper };
        yield return new object[] { TFClass.Demoman, TFSlot.Sapper };
        yield return new object[] { TFClass.Heavy, TFSlot.Sapper };
        yield return new object[] { TFClass.Engineer, TFSlot.Sapper };
        yield return new object[] { TFClass.Medic, TFSlot.Sapper };
        yield return new object[] { TFClass.Sniper, TFSlot.Sapper };
    }

    [Theory]
    [MemberData(nameof(GetAllInvalidLoadoutCombos), MemberType = typeof(RandomizerTests))]
    public static void RandomizingForInvalidLoadoutComboThrowsArgumentException(TFClass targetClass, TFSlot targetSlot)
    {
        var randomizer = new Generator(DbFixture);
        Assert.Throws<ArgumentException>(() =>
        {
            randomizer.RandomizeWeapon(targetClass, targetSlot);
        });
    }

    [Theory]
    [MemberData(nameof(GetAllExistingLoadoutCombos), MemberType = typeof(RandomizerTests))]
    public static void ReturnedWeaponIsOneOfTheExpectedOutcomes(LoadoutCombination loadoutCombo, int attempts)
    {
        var randomizer = new RandomLoadoutGenerator.Generator(DbFixture);
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

    [Fact]
    public static void AllWeaponsAreBeEnabledByDefault()
    {
        var generator = new Generator(DbFixture);
        Assert.Equal(generator.GetAllWeapons().Count(), generator.GetEnabledWeapons().Count());
    }

    [Fact]
    public static void DisabledWeaponsAreNotAnOutcome()
    {
        //Test on spy's sappers
        var generator = new Generator(DbFixture);
        generator.DisableWeapons(126); //Red-Tape Recorder
        generator.DisableWeapons(188, 189); //Stock sapper reskins, not the stock sapper itself - Ap-Sap & Snack Attack
        for (int i = 0; i < 5; i++)
        {
            var result = generator.RandomizeWeapon(TFClass.Spy, TFSlot.Sapper); //This should be 100% the Stock sapper
            Assert.Equal("Sapper", result.Name);
        }
    }

    [Fact]
    //POSSIBLY PROBLEMATIC
    //At normal session it fails, expected value: 197, actual: 194
    //When debugged all is fine, including enabling/disabling specified weapons
    public static void DisablingEnablingWeaponsChangesTheirCount()
    {
        var generator = new Generator(DbFixture);
        generator.DisableWeapons(1, 2, 3);
        Assert.Equal(generator.GetAllWeapons().Count() - 3, generator.GetEnabledWeapons().Count());
        Assert.Equal(3, generator.GetDisabledWeapons().Count());

        generator.EnableWeapons(2, 3);
        Assert.Equal(generator.GetAllWeapons().Count() - 1, generator.GetEnabledWeapons().Count());
        Assert.Single(generator.GetDisabledWeapons());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(69696969)]
    public static void EnablingDisablingInvalidIDsThrowsArgumentExceptions(int invalidId)
    {
        var generator = new Generator(DbFixture);
        Assert.Throws<ArgumentException>(() =>
        {
            generator.DisableWeapons(invalidId);
        });
        Assert.Throws<ArgumentException>(() =>
        {
            generator.EnableWeapons(invalidId);
        });
    }

    [Theory]
    [InlineData(TFClass.Scout, 36)]
    [InlineData(TFClass.Soldier, 34)]
    [InlineData(TFClass.Pyro, 36)]
    [InlineData(TFClass.Demoman, 33)]
    [InlineData(TFClass.Heavy, 32)]
    [InlineData(TFClass.Engineer, 19)]
    [InlineData(TFClass.Medic, 21)]
    [InlineData(TFClass.Sniper, 29)]
    [InlineData(TFClass.Spy, 24)]
    public static void GetEnabledWeaponsReturnsCorrectlyForClasses(TFClass targetClass, int expectedAmount)
    {
        var generator = new Generator(DbFixture);
        Assert.Equal(expectedAmount, generator.GetEnabledWeapons(targetClass).Count());
    }

    [Theory]
    [InlineData(TFSlot.Primary, 61)]
    [InlineData(TFSlot.Secondary, 67)]
    [InlineData(TFSlot.Melee, 132)]
    [InlineData(TFSlot.Sapper, 4)]
    public static void GetEnabledWeaponsReturnsCorrectlyForSlots(TFSlot targetSlot, int expectedAmount)
    {
        var generator = new Generator(DbFixture);
        Assert.Equal(expectedAmount, generator.GetEnabledWeapons(targetSlot).Count());
    }

    [Theory]
    [InlineData(TFClass.Scout, TFSlot.Primary, 6)]
    [InlineData(TFClass.Scout, TFSlot.Secondary, 10)]
    [InlineData(TFClass.Scout, TFSlot.Melee, 20)]
    [InlineData(TFClass.Soldier, TFSlot.Primary, 9)]
    [InlineData(TFClass.Soldier, TFSlot.Secondary, 10)]
    [InlineData(TFClass.Soldier, TFSlot.Melee, 15)]
    [InlineData(TFClass.Pyro, TFSlot.Primary, 7)]
    [InlineData(TFClass.Pyro, TFSlot.Secondary, 9)]
    [InlineData(TFClass.Pyro, TFSlot.Melee, 20)]
    [InlineData(TFClass.Demoman, TFSlot.Primary, 7)]
    [InlineData(TFClass.Demoman, TFSlot.Secondary, 7)]
    [InlineData(TFClass.Demoman, TFSlot.Melee, 19)]
    [InlineData(TFClass.Heavy, TFSlot.Primary, 6)]
    [InlineData(TFClass.Heavy, TFSlot.Secondary, 9)]
    [InlineData(TFClass.Heavy, TFSlot.Melee, 17)]
    [InlineData(TFClass.Engineer, TFSlot.Primary, 6)]
    [InlineData(TFClass.Engineer, TFSlot.Secondary, 6)]
    [InlineData(TFClass.Engineer, TFSlot.Melee, 7)]
    [InlineData(TFClass.Medic, TFSlot.Primary, 4)]
    [InlineData(TFClass.Medic, TFSlot.Secondary, 4)]
    [InlineData(TFClass.Medic, TFSlot.Melee, 13)]
    [InlineData(TFClass.Sniper, TFSlot.Primary, 10)]
    [InlineData(TFClass.Sniper, TFSlot.Secondary, 7)]
    [InlineData(TFClass.Sniper, TFSlot.Melee, 12)]
    [InlineData(TFClass.Spy, TFSlot.Primary, 6)]
    [InlineData(TFClass.Spy, TFSlot.Secondary, 5)]
    [InlineData(TFClass.Spy, TFSlot.Melee, 9)]
    [InlineData(TFClass.Spy, TFSlot.Sapper, 4)]
    public static void GetEnabledWeaponsReturnsCorrectlyForClassesAndSlots(TFClass targetClass, TFSlot targetSlot, int expectedAmount)
    {
        var generator = new Generator(DbFixture);
        Assert.Equal(expectedAmount, generator.GetEnabledWeapons(targetClass, targetSlot).Count());
    }

    [Theory]
    [MemberData(nameof(GetAllInvalidLoadoutCombos), MemberType = typeof(RandomizerTests))]    
    public static void GetEnabledWeaponsForInvalidLoadoutCombosThrowsArugmentException(TFClass targetClass, TFSlot targetSlot)
    {
        var generator = new Generator(DbFixture);
        Assert.Throws<ArgumentException>(() =>
        {
            generator.GetEnabledWeapons(targetClass, targetSlot);
        });
    }
}
