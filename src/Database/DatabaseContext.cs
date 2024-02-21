using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RandomLoadoutGenerator.Models;

namespace RandomLoadoutGenerator.Database;

///<inheritdoc />
public class DatabaseContext : DbContext
{
    private readonly SqliteOpenMode _connectionMode;

    ///<inheritdoc cref="DbSet{TEntity}"/>
    public virtual DbSet<ReskinGroup> ReskinGroups { get; set; }
    ///<inheritdoc cref="DbSet{TEntity}"/>
    public virtual DbSet<LoadoutCombination> LoadoutCombinations { get; set; }
    ///<inheritdoc cref="DbSet{TEntity}"/>
    public virtual DbSet<Weapon> Weapons { get; set; }

    /// <summary>
    /// Creates a new instance. See <see cref="DbContext()"/> for more info.
    /// </summary>
    /// <param name="connectionMode">Determines the connection type. See <see cref="SqliteOpenMode"/> enum for more info.</param>
    public DatabaseContext(SqliteOpenMode connectionMode = SqliteOpenMode.ReadWriteCreate) : base()
    {
        _connectionMode = connectionMode;
    }

    /// <inheritdoc cref="DatabaseContext(SqliteOpenMode)"/>
    /// <param name="optionsBuilder">Allows for additional configuration of the instance through an option builder.</param>
    /// <param name="connectionMode">Determines the connection type. See <see cref="SqliteOpenMode"/> enum for more info.</param>
    public DatabaseContext(DbContextOptionsBuilder optionsBuilder, SqliteOpenMode connectionMode = SqliteOpenMode.ReadWriteCreate) : base(optionsBuilder.Options)
    {
        _connectionMode = connectionMode;
    }

    ///<inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        SqliteConnectionStringBuilder builder = new()
        {
            DataSource = DatabaseFile.FullPath,
            Mode = _connectionMode,
            ForeignKeys = true
        };
        optionsBuilder.UseSqlite(builder.ToString());
    }

    /// <summary>
    /// Purges all the records and repopulates all tables with up-to-date items, classes and slots.<br/>
    /// <b>Calling this method will erase the previous database!</b><br/>
    /// Avoid calling this method from the code. Use it only when a fresh database copy is required.
    /// </summary>
    public static void Purge()
    {
        using var context = new DatabaseContext();
        context.Database.EnsureDeleted();
        context.Database.Migrate();
        context.SaveChanges();

        #region LoadoutCombinations
        //CHANGING THE ORDER WILL BREAK THINGS IN WEAPON LIST UNLESS ADJUSTED ACCORDINGLY
        var loadoutcombos = new List<LoadoutCombination>()
        {
            new(){ Class = TFClass.Scout, Slot = TFSlot.Primary },
            new(){ Class = TFClass.Scout, Slot = TFSlot.Secondary },
            new(){ Class = TFClass.Scout, Slot = TFSlot.Melee },

            new(){ Class = TFClass.Soldier, Slot = TFSlot.Primary },
            new(){ Class = TFClass.Soldier, Slot = TFSlot.Secondary },
            new(){ Class = TFClass.Soldier, Slot = TFSlot.Melee },

            new(){ Class = TFClass.Pyro, Slot = TFSlot.Primary },
            new(){ Class = TFClass.Pyro, Slot = TFSlot.Secondary },
            new(){ Class = TFClass.Pyro, Slot = TFSlot.Melee },

            new(){ Class = TFClass.Demoman, Slot = TFSlot.Primary },
            new(){ Class = TFClass.Demoman, Slot = TFSlot.Secondary },
            new(){ Class = TFClass.Demoman, Slot = TFSlot.Melee },

            new(){ Class = TFClass.Heavy, Slot = TFSlot.Primary },
            new(){ Class = TFClass.Heavy, Slot = TFSlot.Secondary },
            new(){ Class = TFClass.Heavy, Slot = TFSlot.Melee },

            new(){ Class = TFClass.Engineer, Slot = TFSlot.Primary },
            new(){ Class = TFClass.Engineer, Slot = TFSlot.Secondary },
            new(){ Class = TFClass.Engineer, Slot = TFSlot.Melee },

            new(){ Class = TFClass.Medic, Slot = TFSlot.Primary },
            new(){ Class = TFClass.Medic, Slot = TFSlot.Secondary },
            new(){ Class = TFClass.Medic, Slot = TFSlot.Melee },

            new(){ Class = TFClass.Sniper, Slot = TFSlot.Primary },
            new(){ Class = TFClass.Sniper, Slot = TFSlot.Secondary },
            new(){ Class = TFClass.Sniper, Slot = TFSlot.Melee },

            new(){ Class = TFClass.Spy, Slot = TFSlot.Primary },
            new(){ Class = TFClass.Spy, Slot = TFSlot.Secondary },
            new(){ Class = TFClass.Spy, Slot = TFSlot.Melee },
            new(){ Class = TFClass.Spy, Slot = TFSlot.Sapper },
        };
        context.LoadoutCombinations.AddRange(loadoutcombos);
        #endregion

        #region Reskin Groups
        //CHANGING THE ORDER WILL BREAK THINGS IN WEAPON LIST UNLESS ADJUSTED ACCORDINGLY
        var reskinGroups = new List<ReskinGroup>()
        {
            //Scout reskins
            new(){ Name = "MadMilkReskins" }, //0
            new(){ Name = "StockBatReskins" },
            new(){ Name = "BostonBasherReskins" },

            //Soldier reskins
            new(){ Name = "RocketLauncherReskins" },

            //Pyro reskins
            new(){ Name = "FlameThrowerReskins" },
            new(){ Name = "AxtinguisherReskins" }, //5
            new(){ Name = "HomewreckerReskins" },

            //Demoman reskins
            new(){ Name = "BootsReskins" },
            new(){ Name = "BottleReskins" },
            new(){ Name = "EyelanderReskins" },

            //Heavy reskins
            new(){ Name = "MinigunReskins" }, //10
            new(){ Name = "SandvichReskins" },
            new(){ Name = "DalokohsBarReskins" },
            new(){ Name = "StockFistsReskins" },
            new(){ Name = "GRUReskins" },

            //Engineer reskins
            new(){ Name = "WranglerReskins" }, //15
            new(){ Name = "WrenchReskins" },

            //Sniper reskins
            new(){ Name = "SniperRifleReskins" },
            new(){ Name = "BowReskins" },
            new(){ Name = "MachinaReskins" },
            new(){ Name = "PeeReskins" }, //20

            //Spy reskins
            new(){ Name = "RevolverReskins" },
            new(){ Name = "StockInvisWatchReskins" },
            new(){ Name = "StockKnifeReskins" },
            new(){ Name = "YERReskins" },
            new(){ Name = "StockSapperReskins" }, //25

            //Multi-class reskins
            new(){ Name = "StockPistolReskins" },
            new(){ Name = "StockMeleeReskins" }
        };
        context.ReskinGroups.AddRange(reskinGroups);
        #endregion

        #region Weapons
        /* 
         * List of (almost) all weapons in TF2.
         * First sorted by Class, then Slot, then Name.
         * Multi-class weapons are separated from single-class weapons at the bottom.
         * Multi-class weapons are sorted only by Name.
         * Spy's watches are considered as Secondary. Sappers have a separate slot.
         *
         * This list excludes expensive and hard/impossible-to-obtain variants of weapons, such as Golden Frying Pan or Golden Wrench. Excluded from this list are also all PDAs since they have no unlockable variants. Full list of excluded items which the TF2 wiki categorized as distinct weapons:
         * - Golden Wrench
         * - Saxxy
         * - Golden Frying Pan
         * - Memory Maker
         * 
         * Source - https://wiki.teamfortress.com/wiki/Weapons
         */
        var weapons = new List<Weapon>()
        {
            #region Scout-only weapons
            new()
            {
                Name = "Baby Face's Blaster",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[0] },
            },
            new()
            {
                Name = "Back Scatter",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[0] },
            },
            new()
            {
                Name = "Force-a-Nature",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[0] },
            },
            new()
            {
                Name = "Scattergun",
                LoadoutCombos = new LoadoutCombination[]{ loadoutcombos[0] },
                IsStock = true
            },
            new()
            {
                Name = "Shortstop",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[0] },
            },
            new()
            {
                Name = "Soda Popper",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[0] },
            },

            new()
            {
                Name = "Bonk! Atomic Punch",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[1] },
            },
            new()
            {
                Name = "Crit-a-Cola",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[1] },
            },
            new()
            {
                Name = "Flying Guillotine",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[1] },
            },
            new()
            {
                Name = "Mad Milk",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[1] },
                ReskinGroup = reskinGroups[0]
            },
            new()
            {
                Name = "Mutated Milk",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[1] },
                ReskinGroup = reskinGroups[0]
            },
            new()
            {
                Name = "Pretty Boy's Pocket Pistol",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[1] },
            },
            new()
            {
                Name = "Winger",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[1] },
            },

            new()
            {
                Name = "Atomizer",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] }
            },
            new()
            {
                Name = "Bat",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] },
                ReskinGroup = reskinGroups[1],
                IsStock = true
            },
            new()
            {
                Name = "Batsaber",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] },
                ReskinGroup = reskinGroups[1]
            },
            new()
            {
                Name = "Boston Basher",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] },
                ReskinGroup = reskinGroups[2]
            },
            new()
            {
                Name = "Candy Cane",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] },
            },
            new()
            {
                Name = "Fan O'War",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] }
            },
            new()
            {
                Name = "Holy Mackerel",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] },
                ReskinGroup = reskinGroups[1]
            },
            new()
            {
                Name = "Sandman",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] },
            },
            new()
            {
                Name = "Sun-on-a-Stick",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] }
            },
            new()
            {
                Name = "Three-Rune Blade",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] },
                ReskinGroup = reskinGroups[2]
            },
            new()
            {
                Name = "Unarmed Combat",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] },
                ReskinGroup = reskinGroups[1]
            },
            new()
            {
                Name = "Wrap Assassin",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[2] }
            },
            #endregion

            #region Soldier-only weapons
            new()
            {
                Name = "Air Strike",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[3] }
            },
            new()
            {
                Name = "Beggar's Bazooka",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[3] }
            },
            new()
            {
                Name = "Black Box",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[3] }
            },
            new()
            {
                Name = "Cow Mangler 5000",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[3] }
            },
            new()
            {
                Name = "Direct Hit",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[3] }
            },
            new()
            {
                Name = "Liberty Launcher",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[3] }
            },
            new()
            {
                Name = "Original",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[3] },
                ReskinGroup = reskinGroups[3]
            },
            new()
            {
                Name = "Rocket Jumper",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[3] }
            },
            new()
            {
                Name = "Rocket Launcher",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[3] },
                ReskinGroup = reskinGroups[3],
                IsStock = true
            },

            new()
            {
                Name = "Battalion's Backup",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[4] }
            },
            new()
            {
                Name = "Buff Banner",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[4] }
            },
            new()
            {
                Name = "Concheror",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[4] }
            },
            new()
            {
                Name = "Gunboats",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[4] }
            },
            new()
            {
                Name = "Mantreads",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[4] }
            },
            new()
            {
                Name = "Righteous Bison",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[4] }
            },

            new()
            {
                Name = "Disciplinary Action",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[5] }
            },
            new()
            {
                Name = "Equalizer",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[5] }
            },
            new()
            {
                Name = "Escape Plan",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[5] }
            },
            new()
            {
                Name = "Market Gardener",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[5] }
            },
            new()
            {
                Name = "Shovel",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[5] },
                IsStock = true
            },
            #endregion

            #region Pyro-only weapons
            new()
            {
                Name = "Backburner",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[6] }
            },
            new()
            {
                Name = "Degreaser",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[6] }
            },
            new()
            {
                Name = "Dragon's Fury",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[6] }
            },
            new()
            {
                Name = "Flame Thrower",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[6] },
                ReskinGroup = reskinGroups[4],
                IsStock = true
            },
            new()
            {
                Name = "Nostromo Napalmer",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[6] },
                ReskinGroup = reskinGroups[4]
            },
            new()
            {
                Name = "Phlogistinator",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[6] }
            },
            new()
            {
                Name = "Rainblower",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[6] }
            },

            new()
            {
                Name = "Detonator",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[7] }
            },
            new()
            {
                Name = "Flare Gun",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[7] }
            },
            new()
            {
                Name = "Gas Passer",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[7] }
            },
            new()
            {
                Name = "Manmelter",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[7] }
            },
            new()
            {
                Name = "Scorch Shot",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[7] }
            },
            new()
            {
                Name = "Thermal Thruster",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[7] }
            },

            new()
            {
                Name = "Axtinguisher",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] },
                ReskinGroup = reskinGroups[5]
            },
            new()
            {
                Name = "Back Scratcher",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] }
            },
            new()
            {
                Name = "Fire Axe",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] }
            },
            new()
            {
                Name = "Homewrecker",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] },
                ReskinGroup = reskinGroups[6]
            },
            new()
            {
                Name = "Hot Hand",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] }
            },
            new()
            {
                Name = "Lollichop",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] }
            },
            new()
            {
                Name = "Maul",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] },
                ReskinGroup = reskinGroups[6]
            },
            new()
            {
                Name = "Neon Annihilator",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] }
            },
            new()
            {
                Name = "Postal Pummeler",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] },
                ReskinGroup = reskinGroups[5]
            },
            new()
            {
                Name = "Powerjack",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] }
            },
            new()
            {
                Name = "Sharpened Volcano Fragment",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] }
            },
            new()
            {
                Name = "Third Degree",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[8] }
            },
            #endregion

            #region Demoman-only weapons
            new()
            {
                Name = "Ali Baba's Wee Booties",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[9] },
                ReskinGroup = reskinGroups[7]
            },
            new()
            {
                Name = "Bootlegger",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[9] },
                ReskinGroup = reskinGroups[7]
            },
            new()
            {
                Name = "Grenade Launcher",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[9] },
                IsStock = true
            },
            new()
            {
                Name = "Iron Bomber",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[9] }
            },
            new()
            {
                Name = "Loch-n-Load",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[9] }
            },
            new()
            {
                Name = "Loose Cannon",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[9] }
            },

            new()
            {
                Name = "Chargin' Targe",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[10] }
            },
            new()
            {
                Name = "Quickiebomb Launcher",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[10] }
            },
            new()
            {
                Name = "Scottish Resistance",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[10] }
            },
            new()
            {
                Name = "Splendid Screen",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[10] }
            },
            new()
            {
                Name = "Stickybomb Launcher",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[10] },
                IsStock = true
            },
            new()
            {
                Name = "Sticky Jumper",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[10] }
            },
            new()
            {
                Name = "Tide Turner",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[10] }
            },

            new()
            {
                Name = "Bottle",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[11] },
                IsStock = true,
                ReskinGroup = reskinGroups[7]
            },
            new()
            {
                Name = "Claidheamh Mòr",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[11] }
            },
            new()
            {
                Name = "Eyelander",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[11] },
                ReskinGroup = reskinGroups[8]
            },
            new()
            {
                Name = "Horseless Headless Horsemann's Headtaker",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[11] },
                ReskinGroup = reskinGroups[8]
            },
            new()
            {
                Name = "Nessie's Nine Iron",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[11] },
                ReskinGroup = reskinGroups[8]
            },
            new()
            {
                Name = "Persian Persuader",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[11] }
            },
            new()
            {
                Name = "Scotsman's Skullcutter",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[11] }
            },
            new()
            {
                Name = "Scottish Handshake",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[11] },
                ReskinGroup = reskinGroups[7]
            },
            new()
            {
                Name = "Ullapool Caber",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[11] }
            },
            #endregion

            #region Heavy-only weapons
            new()
            {
                Name = "Brass Beast",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[12] }
            },
            new()
            {
                Name = "Huo-Long Heater",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[12] }
            },
            new()
            {
                Name = "Iron Curtain",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[12] },
                ReskinGroup = reskinGroups[10]
            },
            new()
            {
                Name = "Minigun",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[12] },
                ReskinGroup = reskinGroups[10],
                IsStock = true,
            },
            new()
            {
                Name = "Natascha",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[12] }
            },
            new()
            {
                Name = "Tomislav",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[12] }
            },

            new()
            {
                Name = "Buffalo Steak Sandvich",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[13] }
            },
            new()
            {
                Name = "Dalokohs Bar",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[13] },
                ReskinGroup = reskinGroups[12]
            },
            new()
            {
                Name = "Family Business",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[13] }
            },
            new()
            {
                Name = "Fishcake",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[13] },
                ReskinGroup = reskinGroups[12]
            },
            new()
            {
                Name = "Robo-Sandvich",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[13] },
                ReskinGroup = reskinGroups[11]
            },
            new()
            {
                Name = "Sandvich",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[13] },
                ReskinGroup = reskinGroups[11]
            },
            new()
            {
                Name = "Second Banana",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[13] }
            },

            new()
            {
                Name = "Apoco-Fists",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[14] },
                ReskinGroup = reskinGroups[13]
            },
            new()
            {
                Name = "Bread Bite",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[14] },
                ReskinGroup = reskinGroups[14]
            },
            new()
            {
                Name = "Eviction Notice",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[14] }
            },
            new()
            {
                Name = "Fists",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[14] },
                ReskinGroup = reskinGroups[13],
                IsStock = true
            },
            new()
            {
                Name = "Fists of Steel",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[14] }
            },
            new()
            {
                Name = "Gloves of Running Urgently",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[14] },
                ReskinGroup = reskinGroups[14]
            },
            new()
            {
                Name = "Holiday Punch",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[14] }
            },
            new()
            {
                Name = "Killing Gloves of Boxing",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[14] }
            },
            new()
            {
                Name = "Warrior's Spirit",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[14] }
            },
            #endregion

            #region Engineer-only weapons
            new()
            {
                Name = "Frontier Justice",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[15] }
            },
            new()
            {
                Name = "Pomson 6000",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[15] }
            },
            new()
            {
                Name = "Rescue Ranger",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[15] }
            },
            new()
            {
                Name = "Widowmaker",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[15] }
            },

            new()
            {
                Name = "Giger Counter",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[16] },
                ReskinGroup = reskinGroups[15]
            },
            new()
            {
                Name = "Short Circuit",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[16] }
            },
            new()
            {
                Name = "Wrangler",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[16] },
                ReskinGroup = reskinGroups[15]
            },

            new()
            {
                Name = "Eureka Effect",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[17] }
            },
            new()
            {
                Name = "Gunslinger",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[17] }
            },
            new()
            {
                Name = "Jag",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[17] }
            },
            new()
            {
                Name = "Southern Hospitality",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[17] }
            },
            new()
            {
                Name = "Wrench",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[17] },
                ReskinGroup = reskinGroups[16],
                IsStock = true
            },
            #endregion

            #region Medic-only weapons
            new()
            {
                Name = "Blutsauger",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[18] }
            },
            new()
            {
                Name = "Crussader's Crossbow",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[18] }
            },
            new()
            {
                Name = "Overdose",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[18] }
            },
            new()
            {
                Name = "Syringe Gun",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[18] },
                IsStock = true
            },

            new()
            {
                Name = "Kritzkrieg",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[19] }
            },
            new()
            {
                Name = "Medi Gun",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[19] },
                IsStock = true
            },
            new()
            {
                Name = "Quick-Fix",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[19] }
            },
            new()
            {
                Name = "Vaccinator",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[19] }
            },

            new()
            {
                Name = "Amputator",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[20] }
            },
            new()
            {
                Name = "Bonesaw",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[20] },
                IsStock = true
            },
            new()
            {
                Name = "Solemn Vow",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[20] }
            },
            new()
            {
                Name = "Ubersaw",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[20] }
            },
            new()
            {
                Name = "Vita-Saw",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[20] }
            },
            #endregion

            #region Sniper-only weapons
            new()
            {
                Name = "AWPer Hand",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] },
                ReskinGroup = reskinGroups[17]
            },
            new()
            {
                Name = "Bazaar Bargain",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] }
            },
            new()
            {
                Name = "Classic",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] }
            },
            new()
            {
                Name = "Fortified Compound",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] },
                ReskinGroup = reskinGroups[18]
            },
            new()
            {
                Name = "Hitman's Heatmaker",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] }
            },
            new()
            {
                Name = "Huntsman",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] },
                ReskinGroup = reskinGroups[18]
            },
            new()
            {
                Name = "Machina",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] },
                ReskinGroup = reskinGroups[19]
            },
            new()
            {
                Name = "Shooting Star",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] },
                ReskinGroup = reskinGroups[19]
            },
            new()
            {
                Name = "Sniper Rifle",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] },
                ReskinGroup = reskinGroups[17],
                IsStock = true
            },
            new()
            {
                Name = "Sydner Sleeper",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[21] }
            },

            new()
            {
                Name = "Cleaner's Carbine",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[22] }
            },
            new()
            {
                Name = "Cozy Camper",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[22] }
            },
            new()
            {
                Name = "Darwin's Danger Shield",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[22] }
            },
            new()
            {
                Name = "Jarate",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[22] },
                ReskinGroup = reskinGroups[20]
            },
            new()
            {
                Name = "Razorback",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[22] }
            },
            new()
            {
                Name = "Self-Aware Beauty Mark",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[22] },
                ReskinGroup = reskinGroups[20]
            },
            new()
            {
                Name = "SMG",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[22] },
                IsStock = true
            },

            new()
            {
                Name = "Bushwacka",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[23] }
            },
            new()
            {
                Name = "Kukri",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[23] },
                IsStock = true
            },
            new()
            {
                Name = "Shahanshah",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[23] }
            },
            new()
            {
                Name = "Tribalman's Shiv",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[23] }
            },
            #endregion

            #region Spy-only weapons
            new()
            {
                Name = "Ambassador",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[24] }
            },
            new()
            {
                Name = "Big Kill",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[24] },
                ReskinGroup = reskinGroups[21]
            },
            new()
            {
                Name = "Diamondback",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[24] }
            },
            new()
            {
                Name = "Enforcer",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[24] }
            },
            new()
            {
                Name = "L'Etranger",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[24] }
            },
            new()
            {
                Name = "Revolver",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[24] },
                ReskinGroup = reskinGroups[21],
                IsStock = true
            },

            new()
            {
                Name = "Cloak and Dagger",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[25] }
            },
            new()
            {
                Name = "Dead Ringer",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[25] }
            },
            new()
            {
                Name = "Enthusiast's Timepiece",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[25] },
                ReskinGroup = reskinGroups[22]
            },
            new()
            {
                Name = "Invis Watch",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[25] },
                ReskinGroup = reskinGroups[22],
                IsStock = true
            },
            new()
            {
                Name = "Quäckenbirdt",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[25] },
                ReskinGroup = reskinGroups[22]
            },

            new()
            {
                Name = "Big Earner",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[26] }
            },
            new()
            {
                Name = "Black Rose",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[26] },
                ReskinGroup = reskinGroups[23]
            },
            new()
            {
                Name = "Conniver's Kunai",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[26] }
            },
            new()
            {
                Name = "Knife",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[26] },
                ReskinGroup = reskinGroups[23],
                IsStock = true
            },
            new()
            {
                Name = "Sharp Dresser",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[26] },
                ReskinGroup = reskinGroups[23]
            },
            new()
            {
                Name = "Spy-cicle",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[26] }
            },
            new()
            {
                Name = "Wanga Prick",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[26] },
                ReskinGroup = reskinGroups[24]
            },
            new()
            {
                Name = "Your Eternal Reward",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[26] },
                ReskinGroup = reskinGroups[24]
            },

            new()
            {
                Name = "Sapper",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[27] },
                ReskinGroup = reskinGroups[25],
                IsStock = true
            },
            new()
            {
                Name = "Ap-Sap",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[27] },
                ReskinGroup = reskinGroups[25]
            },
            new()
            {
                Name = "Snack Attack",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[27] },
                ReskinGroup = reskinGroups[25]
            },
            new()
            {
                Name = "Red-Tape Recorder",
                LoadoutCombos = new LoadoutCombination[] { loadoutcombos[27] }
            },
            #endregion

            #region Multi-class weapons
            new()
            {
                Name = "B.A.S.E. Jumper",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[4],
                    loadoutcombos[9]
                },
            },
            new()
            {
                Name = "Bat Outta Hell",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[2],
                    loadoutcombos[5],
                    loadoutcombos[8],
                    loadoutcombos[11],
                    loadoutcombos[14],
                    loadoutcombos[20],
                    loadoutcombos[23]
                },
                ReskinGroup = reskinGroups[27]
            },
            new()
            {
                Name = "C.A.P.P.E.R",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[1],
                    loadoutcombos[16]
                },
                ReskinGroup = reskinGroups[26]
            },
            new()
            {
                Name = "Conscientious Objector",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[2],
                    loadoutcombos[5],
                    loadoutcombos[8],
                    loadoutcombos[11],
                    loadoutcombos[14],
                    loadoutcombos[20],
                    loadoutcombos[23]
                },
                ReskinGroup = reskinGroups[27]
            },
            new()
            {
                Name = "Crossing Guard",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[2],
                    loadoutcombos[5],
                    loadoutcombos[8],
                    loadoutcombos[11],
                    loadoutcombos[14],
                    loadoutcombos[20],
                    loadoutcombos[23]
                },
                ReskinGroup = reskinGroups[27]
            },
            new()
            {
                Name = "Freedom Staff",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[2],
                    loadoutcombos[5],
                    loadoutcombos[8],
                    loadoutcombos[11],
                    loadoutcombos[14],
                    loadoutcombos[20],
                    loadoutcombos[23]
                },
                ReskinGroup = reskinGroups[27]
            },
            new()
            {
                Name = "Frying Pan",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[2],
                    loadoutcombos[5],
                    loadoutcombos[8],
                    loadoutcombos[11],
                    loadoutcombos[14],
                    loadoutcombos[20],
                    loadoutcombos[23]
                },
                ReskinGroup = reskinGroups[27]
            },
            new()
            {
                Name = "Half-Zatoichi",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[5],
                    loadoutcombos[11]
                }
            },
            new()
            {
                Name = "Ham Shank",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[2],
                    loadoutcombos[5],
                    loadoutcombos[8],
                    loadoutcombos[11],
                    loadoutcombos[14],
                    loadoutcombos[20],
                    loadoutcombos[23]
                },
                ReskinGroup = reskinGroups[27]
            },
            new()
            {
                Name = "Necro Smasher",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[2],
                    loadoutcombos[5],
                    loadoutcombos[8],
                    loadoutcombos[11],
                    loadoutcombos[14],
                    loadoutcombos[17],
                    loadoutcombos[20],
                    loadoutcombos[23]
                },
                ReskinGroup = reskinGroups[27]
            },
            new()
            {
                Name = "Lugermorph",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[1],
                    loadoutcombos[16]
                },
                ReskinGroup = reskinGroups[26]
            },
            new()
            {
                Name = "Pain Train",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[5],
                    loadoutcombos[11]
                }
            },
            new()
            {
                Name = "Panic Attack",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[4],
                    loadoutcombos[7],
                    loadoutcombos[13],
                    loadoutcombos[15]
                }
            },
            new()
            {
                Name = "Pistol",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[1],
                    loadoutcombos[16]
                },
                ReskinGroup = reskinGroups[26],
                IsStock = true
            },
            new()
            {
                Name = "Prinny Machete",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[2],
                    loadoutcombos[5],
                    loadoutcombos[8],
                    loadoutcombos[11],
                    loadoutcombos[14],
                    loadoutcombos[17],
                    loadoutcombos[20],
                    loadoutcombos[23],
                    loadoutcombos[26]
                },
                ReskinGroup = reskinGroups[27]
            },
            new()
            {
                Name = "Reserve Shooter",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[4],
                    loadoutcombos[7]
                },
            },
            new()
            {
                Name = "Shotgun",
                LoadoutCombos = new LoadoutCombination[]
                {
                    loadoutcombos[4],
                    loadoutcombos[7],
                    loadoutcombos[13],
                    loadoutcombos[15]
                },
                IsStock = true
            },
            #endregion
        };
        context.Weapons.AddRange(weapons);
        #endregion

        context.SaveChanges();
    }
}
