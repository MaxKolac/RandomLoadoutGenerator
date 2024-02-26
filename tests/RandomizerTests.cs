using RandomLoadoutGenerator;
using RandomLoadoutGenerator.Database;
using RandomLoadoutGenerator.Models;

namespace RandomLoadoutGeneratorTests;

public class RandomizerTests
{

    public static IEnumerable<object[]> GetAllLoadoutCombos()
    {
        foreach (var combo in DbFixture.LoadoutCombinations)
        {
            yield return new object[] { combo };
        }
    }

    public static IEnumerable<object[]> GetInvalidLoadoutCombos()
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

    public static IEnumerable<object[]> GetEnabledWeaponsCountPerClass()
    {
        yield return new object[] { TFClass.Scout, 36 };
        yield return new object[] { TFClass.Soldier, 34 };
        yield return new object[] { TFClass.Pyro, 36 };
        yield return new object[] { TFClass.Demoman, 33 };
        yield return new object[] { TFClass.Heavy, 32 };
        yield return new object[] { TFClass.Engineer, 19 };
        yield return new object[] { TFClass.Medic, 21 };
        yield return new object[] { TFClass.Sniper, 29 };
        yield return new object[] { TFClass.Spy, 24 };
    }

    public static IEnumerable<object[]> GetEnabledWeaponsCountPerSlot()
    {
        yield return new object[] { TFSlot.Primary, 61 };
        yield return new object[] { TFSlot.Secondary, 59 };
        yield return new object[] { TFSlot.Melee, 79 };
        yield return new object[] { TFSlot.Sapper, 4 };
    }

    public static IEnumerable<object[]> GetEnabledWeaponsCountPerClassAndSlot()
    {
        yield return new object[] { TFClass.Scout, TFSlot.Primary, 6 };
        yield return new object[] { TFClass.Scout, TFSlot.Secondary, 10 };
        yield return new object[] { TFClass.Scout, TFSlot.Melee, 20 };
        yield return new object[] { TFClass.Soldier, TFSlot.Primary, 9 };
        yield return new object[] { TFClass.Soldier, TFSlot.Secondary, 10 };
        yield return new object[] { TFClass.Soldier, TFSlot.Melee, 15 };
        yield return new object[] { TFClass.Pyro, TFSlot.Primary, 7 };
        yield return new object[] { TFClass.Pyro, TFSlot.Secondary, 9 };
        yield return new object[] { TFClass.Pyro, TFSlot.Melee, 20 };
        yield return new object[] { TFClass.Demoman, TFSlot.Primary, 7 };
        yield return new object[] { TFClass.Demoman, TFSlot.Secondary, 7 };
        yield return new object[] { TFClass.Demoman, TFSlot.Melee, 19 };
        yield return new object[] { TFClass.Heavy, TFSlot.Primary, 6 };
        yield return new object[] { TFClass.Heavy, TFSlot.Secondary, 9 };
        yield return new object[] { TFClass.Heavy, TFSlot.Melee, 17 };
        yield return new object[] { TFClass.Engineer, TFSlot.Primary, 6 };
        yield return new object[] { TFClass.Engineer, TFSlot.Secondary, 6 };
        yield return new object[] { TFClass.Engineer, TFSlot.Melee, 7 };
        yield return new object[] { TFClass.Medic, TFSlot.Primary, 4 };
        yield return new object[] { TFClass.Medic, TFSlot.Secondary, 4 };
        yield return new object[] { TFClass.Medic, TFSlot.Melee, 13 };
        yield return new object[] { TFClass.Sniper, TFSlot.Primary, 10 };
        yield return new object[] { TFClass.Sniper, TFSlot.Secondary, 7 };
        yield return new object[] { TFClass.Sniper, TFSlot.Melee, 12 };
        yield return new object[] { TFClass.Spy, TFSlot.Primary, 6 };
        yield return new object[] { TFClass.Spy, TFSlot.Secondary, 5 };
        yield return new object[] { TFClass.Spy, TFSlot.Melee, 9 };
        yield return new object[] { TFClass.Spy, TFSlot.Sapper, 4 };
    }

    public static IEnumerable<object[]> GetDisabledWeaponsCountPerClass()
    {
        yield return new object[] { TFClass.Scout, 3 };
        yield return new object[] { TFClass.Soldier, 3 };
        yield return new object[] { TFClass.Pyro, 3 };
        yield return new object[] { TFClass.Demoman, 3 };
        yield return new object[] { TFClass.Heavy, 3 };
        yield return new object[] { TFClass.Engineer, 3 };
        yield return new object[] { TFClass.Medic, 3 };
        yield return new object[] { TFClass.Sniper, 3 };
        yield return new object[] { TFClass.Spy, 2 };
    }

    public static IEnumerable<object[]> GetDisabledWeaponsCountPerSlot()
    {
        yield return new object[] { TFSlot.Primary, 0 };
        yield return new object[] { TFSlot.Secondary, 0 };
        yield return new object[] { TFSlot.Melee, 4 };
        yield return new object[] { TFSlot.Sapper, 0 };
    }

    public static IEnumerable<object[]> GetDisabledWeaponsCountPerClassAndSlot()
    {
        yield return new object[] { TFClass.Scout, TFSlot.Primary, 0 };
        yield return new object[] { TFClass.Scout, TFSlot.Secondary, 0 };
        yield return new object[] { TFClass.Scout, TFSlot.Melee, 3 };
        yield return new object[] { TFClass.Soldier, TFSlot.Primary, 0 };
        yield return new object[] { TFClass.Soldier, TFSlot.Secondary, 0 };
        yield return new object[] { TFClass.Soldier, TFSlot.Melee, 3 };
        yield return new object[] { TFClass.Pyro, TFSlot.Primary, 0 };
        yield return new object[] { TFClass.Pyro, TFSlot.Secondary, 0 };
        yield return new object[] { TFClass.Pyro, TFSlot.Melee, 3 };
        yield return new object[] { TFClass.Demoman, TFSlot.Primary, 0 };
        yield return new object[] { TFClass.Demoman, TFSlot.Secondary, 0 };
        yield return new object[] { TFClass.Demoman, TFSlot.Melee, 3 };
        yield return new object[] { TFClass.Heavy, TFSlot.Primary, 0 };
        yield return new object[] { TFClass.Heavy, TFSlot.Secondary, 0 };
        yield return new object[] { TFClass.Heavy, TFSlot.Melee, 3 };
        yield return new object[] { TFClass.Engineer, TFSlot.Primary, 0 };
        yield return new object[] { TFClass.Engineer, TFSlot.Secondary, 0 };
        yield return new object[] { TFClass.Engineer, TFSlot.Melee, 3 };
        yield return new object[] { TFClass.Medic, TFSlot.Primary, 0 };
        yield return new object[] { TFClass.Medic, TFSlot.Secondary, 0 };
        yield return new object[] { TFClass.Medic, TFSlot.Melee, 3 };
        yield return new object[] { TFClass.Sniper, TFSlot.Primary, 0 };
        yield return new object[] { TFClass.Sniper, TFSlot.Secondary, 0 };
        yield return new object[] { TFClass.Sniper, TFSlot.Melee, 3 };
        yield return new object[] { TFClass.Spy, TFSlot.Primary, 0 };
        yield return new object[] { TFClass.Spy, TFSlot.Secondary, 0 };
        yield return new object[] { TFClass.Spy, TFSlot.Melee, 2 };
        yield return new object[] { TFClass.Spy, TFSlot.Sapper, 0 };
    }

    public static IEnumerable<object[]> GetAllWeaponsCountPerClass()
    {
        yield return new object[] { TFClass.Scout, 39 };
        yield return new object[] { TFClass.Soldier, 37 };
        yield return new object[] { TFClass.Pyro, 39 };
        yield return new object[] { TFClass.Demoman, 36 };
        yield return new object[] { TFClass.Heavy, 35 };
        yield return new object[] { TFClass.Engineer, 22 };
        yield return new object[] { TFClass.Medic, 24 };
        yield return new object[] { TFClass.Sniper, 32 };
        yield return new object[] { TFClass.Spy, 26 };
    }

    public static IEnumerable<object[]> GetAllWeaponsCountPerSlot()
    {
        yield return new object[] { TFSlot.Primary, 61 };
        yield return new object[] { TFSlot.Secondary, 59 };
        yield return new object[] { TFSlot.Melee, 83 };
        yield return new object[] { TFSlot.Sapper, 4 };
    }

    public static IEnumerable<object[]> GetAllWeaponsCountPerClassAndSlot()
    {
        yield return new object[] { TFClass.Scout, TFSlot.Primary, 6 };
        yield return new object[] { TFClass.Scout, TFSlot.Secondary, 10 };
        yield return new object[] { TFClass.Scout, TFSlot.Melee, 23 };
        yield return new object[] { TFClass.Soldier, TFSlot.Primary, 9 };
        yield return new object[] { TFClass.Soldier, TFSlot.Secondary, 10 };
        yield return new object[] { TFClass.Soldier, TFSlot.Melee, 18 };
        yield return new object[] { TFClass.Pyro, TFSlot.Primary, 7 };
        yield return new object[] { TFClass.Pyro, TFSlot.Secondary, 9 };
        yield return new object[] { TFClass.Pyro, TFSlot.Melee, 23 };
        yield return new object[] { TFClass.Demoman, TFSlot.Primary, 7 };
        yield return new object[] { TFClass.Demoman, TFSlot.Secondary, 7 };
        yield return new object[] { TFClass.Demoman, TFSlot.Melee, 22 };
        yield return new object[] { TFClass.Heavy, TFSlot.Primary, 6 };
        yield return new object[] { TFClass.Heavy, TFSlot.Secondary, 9 };
        yield return new object[] { TFClass.Heavy, TFSlot.Melee, 20 };
        yield return new object[] { TFClass.Engineer, TFSlot.Primary, 6 };
        yield return new object[] { TFClass.Engineer, TFSlot.Secondary, 6 };
        yield return new object[] { TFClass.Engineer, TFSlot.Melee, 10 };
        yield return new object[] { TFClass.Medic, TFSlot.Primary, 4 };
        yield return new object[] { TFClass.Medic, TFSlot.Secondary, 4 };
        yield return new object[] { TFClass.Medic, TFSlot.Melee, 16 };
        yield return new object[] { TFClass.Sniper, TFSlot.Primary, 10 };
        yield return new object[] { TFClass.Sniper, TFSlot.Secondary, 7 };
        yield return new object[] { TFClass.Sniper, TFSlot.Melee, 15 };
        yield return new object[] { TFClass.Spy, TFSlot.Primary, 6 };
        yield return new object[] { TFClass.Spy, TFSlot.Secondary, 5 };
        yield return new object[] { TFClass.Spy, TFSlot.Melee, 11 };
        yield return new object[] { TFClass.Spy, TFSlot.Sapper, 4 };
    }

    #region Core Functionality
    [Theory]
    [MemberData(nameof(GetInvalidLoadoutCombos), MemberType = typeof(RandomizerTests))]
    public static void RandomizingForInvalidLoadoutComboThrowsArgumentException(TFClass targetClass, TFSlot targetSlot)
    {
        var randomizer = new Generator(DbFixture);
        Assert.Throws<ArgumentException>(() =>
        {
            randomizer.RandomizeWeapon(targetClass, targetSlot);
        });
    }

    [Theory]
    [MemberData(nameof(GetAllLoadoutCombos), MemberType = typeof(RandomizerTests))]
    public static void ReturnedWeaponIsOneOfTheExpectedOutcomes(LoadoutCombination loadoutCombo)
    {
        const int attempts = 5;
        var randomizer = new Generator();
        var expectedWeapons = from weapon in new DatabaseContext().Weapons
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
    public static void AllWeaponsExceptFourAreEnabledByDefault()
    {
        var generator = new Generator(DbFixture);
        Assert.Equal(generator.GetAllWeapons().Count() - 4, generator.GetEnabledWeapons().Count());
        Assert.Equal(4, generator.GetDisabledWeapons().Count());
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
    public static void EnablingDisablingWeaponsChangesTheirCount()
    {
        var generator = new Generator(DbFixture);
        generator.DisableWeapons(1, 2, 3);
        Assert.Equal(generator.GetAllWeapons().Count() - 7, generator.GetEnabledWeapons().Count());
        Assert.Equal(7, generator.GetDisabledWeapons().Count());

        generator.EnableWeapons(2, 3);
        Assert.Equal(generator.GetAllWeapons().Count() - 5, generator.GetEnabledWeapons().Count());
        Assert.Equal(5, generator.GetDisabledWeapons().Count());
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
    [MemberData(nameof(GetEnabledWeaponsCountPerClass), MemberType = typeof(RandomizerTests))]
    public static void GetEnabledWeaponsReturnsCorrectlyForClasses(TFClass targetClass, int expectedAmount)
    {
        var generator = new Generator(DbFixture);
        Assert.Equal(expectedAmount, generator.GetEnabledWeapons(targetClass).Count());
    }

    [Theory]
    [MemberData(nameof(GetEnabledWeaponsCountPerSlot), MemberType = typeof(RandomizerTests))]
    public static void GetEnabledWeaponsReturnsCorrectlyForSlots(TFSlot targetSlot, int expectedAmount)
    {
        var generator = new Generator(DbFixture);
        Assert.Equal(expectedAmount, generator.GetEnabledWeapons(targetSlot).Count());
    }

    [Theory]
    [MemberData(nameof(GetEnabledWeaponsCountPerClassAndSlot), MemberType = typeof(RandomizerTests))]
    public static void GetEnabledWeaponsReturnsCorrectlyForClassesAndSlots(TFClass targetClass, TFSlot targetSlot, int expectedAmount)
    {
        var generator = new Generator(DbFixture);
        Assert.Equal(expectedAmount, generator.GetEnabledWeapons(targetClass, targetSlot).Count());
    }

    [Theory]
    [MemberData(nameof(GetInvalidLoadoutCombos), MemberType = typeof(RandomizerTests))]
    public static void GetEnabledWeaponsForInvalidLoadoutCombosThrowsArugmentException(TFClass targetClass, TFSlot targetSlot)
    {
        var generator = new Generator(DbFixture);
        Assert.Throws<ArgumentException>(() =>
        {
            generator.GetEnabledWeapons(targetClass, targetSlot);
        });
    }
    #endregion

    #region GetAllWeapons tests
    [Theory]
    [MemberData(nameof(GetAllWeaponsCountPerClass), MemberType = typeof(RandomizerTests))]
    public static void GetAllWeaponsReturnsCorrectlyForClasses(TFClass targetClass, int expectedAmount)
    {
        var generator = new Generator();
        Assert.Equal(expectedAmount, generator.GetAllWeapons(targetClass).Count());
    }

    [Theory]
    [MemberData(nameof(GetAllWeaponsCountPerSlot), MemberType = typeof(RandomizerTests))]
    public static void GetAllWeaponsReturnsCorrectlyForSlots(TFSlot targetSlot, int expectedAmount)
    {
        var generator = new Generator();
        Assert.Equal(expectedAmount, generator.GetAllWeapons(targetSlot).Count());
    }

    [Theory]
    [MemberData(nameof(GetAllWeaponsCountPerClassAndSlot), MemberType = typeof(RandomizerTests))]
    public static void GetAllWeaponsReturnsCorrectlyForClassesAndSlots(TFClass targetClass, TFSlot targetSlot, int expectedAmount)
    {
        var generator = new Generator();
        Assert.Equal(expectedAmount, generator.GetAllWeapons(targetClass, targetSlot).Count());
    }

    [Theory]
    [MemberData(nameof(GetInvalidLoadoutCombos), MemberType = typeof(RandomizerTests))]
    public static void GetAllWeaponsForInvalidLoadoutCombosThrowsArugmentException(TFClass targetClass, TFSlot targetSlot)
    {
        var generator = new Generator();
        Assert.Throws<ArgumentException>(() =>
        {
            generator.GetAllWeapons(targetClass, targetSlot);
        });
    }
    #endregion
}
