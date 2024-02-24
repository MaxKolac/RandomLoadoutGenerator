## Overview

A C# library that allows you to choose random weapons from all weapons available in Team Fortress 2.
You can specify for what class and for what slot should a weapon be randomized.

This library supports weapons that are multi-class and are equippable in different slots, for example The Panic Attack or The B.A.S.E. Jumper.

It's also possible to tell the `Generator` to treat weapon reskins as a single weapon to increase the odds of rolling mechanically different unlockable weapons.
This way you can minimize the odds of pulling different melee reskins 5 times in a row.

You can also just remove weapons from being a possible outcome.

## Boring details

It uses a small built-in SQLite packaged as an embedded resource. 
When you create a `Generator` instance, it unpacks this database into the `Environment.CurrentDirectory` (usually the same directory where your app's executable is).
This database is queried once for every `Generator` instance. Any changes in that `Generator` instance will not be preserved inside the database file.

TODO: Turns those weapons into disabled by default
The database contains (almost) all weapons in Team Fortress 2 listed on the [official Wiki's page](https://wiki.teamfortress.com/wiki/Weapons) . A few expensive and hard/impossible-to-obtain variants were not included in the database. These are:
 - Golden Wrench
 - Saxxy
 - Golden Frying Pan
 - Memory Maker

All weapons follow the `TFSlot.Primary`, `TFSlot.Secondary` and `TFSlot.Melee` slots the same way they do in Team Fortress 2. The exception are Spy's invisibility watches, they are treated as `TFSlot.Secondary`. Spy's sappers are the only weapons to use `TFSlot.Sapper` slot.

## Usage

Just create a new `Generator` instance and use its methods:

```
var generator = new Generator();
var weapon = generator.RandomizeWeapon(TFClass.Scout, TFSlot.Primary);
Console.WriteLine(weapon.ToString());
```

To randomize things like classes and loadout slots, `Generator` has static methods for it.
The `Generator.RandomizeSlot()` method by default won't output a `TFSlot.Sapper`, unless you supply a `true` argument.
The reason why is because telling `Generator` to randomize a sapper for Pyro will throw `ArgumentException`.

```
using RandomLoadoutGenerator;

var randomClass = Generator.RandomizeClass();
var randomSlot = Generator.RandomizeSlot();         
var randomSlot2 = Generator.RandomizeSlot(true);    //has a chance to be a TFSlot.Sapper
```

You can enable `Generator` to treat weapon reskins as a single weapon and have higher chances of returning a mechanically unique weapon by adding `true` as last 3rd argument.

```
var weapon = generator.RandomizeWeapon(TFClass.Scout, TFSlot.Melee, true);
```

You can disable and enable weapons to completely disallow them from being a possible outcome. 
You can choose the weapon to enable/disable by passing its ID. You can either look into the database itself or reference the ID table at the bottom of this README.

All weapons are enabled by default. Keep in mind that disabled weapons will be disabled only for that instance. A new `Generator` instance will have all weapons re-enabled.

```
generator.EnableWeapon(0, 3);
generator.DisableWeapon(1, 2, 3, 4);
```

You can also take a look at `examples` folder on the GitHub repository for an example project. It's a simple console app that generates a new random loadout for a random class everytime you press Enter.

## Weapon ID table

| ID | Name |
|---|---|
| 1  |Baby Face's Blaster|
| 2  |Back Scatter|
| 3  |Force-a-Nature|
| 4  |Scattergun|
| 5  |Shortstop|
| 6  |Soda Popper|
| 7  |Bonk! Atomic Punch|
| 8  |Crit-a-Cola|
| 9  |Flying Guillotine
| 10 |Pretty Boy's Pocket Pistol|
| 11 |Winger|
| 12 |Atomizer|
| 13 |Candy Cane|
| 14 |Fan O'War|
| 15 |Sandman|
| 16 |Sun-on-a-Stick|
| 17 |Wrap Assassin|
| 18 |Air Strike|
| 19 |Beggar's Bazooka|
| 20 |Black Box|
| 21 |Cow Mangler 5000|
| 22 |Direct Hit|
| 23 |Liberty Launcher|
| 24 |Rocket Jumper|
| 25 |Battalion's Backup|
| 26 |Buff Banner|
| 27 |Concheror|
| 28 |Gunboats|
| 29 |Mantreads|
| 30 |Righteous Bison|
| 31 |Disciplinary Action|
| 32 |Equalizer|
| 33 |Escape Plan|
| 34 |Market Gardener|
| 35 |Shovel|
| 36 |Backburner|
| 37 |Degreaser|
| 38 |Dragon's Fury|
| 39 |Phlogistinator|
| 40 |Rainblower|
| 41 |Detonator|
| 42 |Flare Gun|
| 43 |Gas Passer|
| 44 |Manmelter|
| 45 |Scorch Shot|
| 46 |Thermal Thruster|
| 47 |Back Scratcher|
| 48 |Fire Axe|
| 49 |Hot Hand|
| 50 |Lollichop|
| 51 |Neon Annihilator|
| 52 |Powerjack|
| 53 |Sharpened Volcano Fragment|
| 54 |Third Degree|
| 55 |Grenade Launcher|
| 56 |Iron Bomber|
| 57 |Loch-n-Load|
| 58 |Loose Cannon|
| 59 |Chargin' Targe|
| 60 |Quickiebomb Launcher|
| 61 |Scottish Resistance|
| 62 |Splendid Screen|
| 63 |Stickybomb Launcher|
| 64 |Sticky Jumper|
| 65 |Tide Turner|
| 66 |Claidheamh Mòr|
| 67 |Persian Persuader|
| 68 |Scotsman's Skullcuter|
| 69 |Ullapool Caber|
| 70 |Brass Beast|
| 71 |Huo-Long Heater|
| 72 |Natascha|
| 73 |Tomislav|
| 74 |Buffalo Steak Sandvich|
| 75 |Family Business|
| 76 |Second Banana|
| 77 |Eviction Notice|
| 78 |Fists of Steel|
| 79 |Holiday Punch|
| 80 |Killing Gloves of Boxing|
| 81 |Warrior's Spirit|
| 82 |Frontier Justice|
| 83 |Pomson 6000|
| 84 |Rescue Ranger|
| 85 |Widowmaker|
| 86 |Short Circuit|
| 87 |Eureka Effect|
| 88 |Gunslinger|
| 89 |Jag|
| 90 |Southern Hospitality|
| 91 |Blutsauger|
| 92 |Crussader's Crossbow|
| 93 |Overdose|
| 94 |Syringe Gun|
| 95 |Kritzkrieg|
| 96 |Medi Gun|
| 97 |Quick-Fix
| 98 |Vaccinator|
| 99 |Amputator|
| 100|Bonesaw|
| 101|Solemn Vow|
| 102|Ubersaw|
| 103|Vita-Saw|
| 104|Bazaar Bargain|
| 105|Classic|
| 106|Hitman's Heatmaker|
| 107|Sydney Sleeper|
| 108|Cleaner's Carbine|
| 109|Cozy Camper|
| 110|Darwin's Danger Shield|
| 111|Razorback|
| 112|SMG|
| 113|Bushwacka|
| 114|Kukri|
| 115|Shahanshah|
| 116|Tribalman's Shiv|
| 117|Ambassador|
| 118|Diamondback|
| 119|Enforcer|
| 120|L'Etranger|
| 121|Cloak and Dagger|
| 122|Dead Ringer|
| 123|Big Earner|
| 124|Conniver's Kunai|
| 125|Spy-cicle|
| 126|Red-Tape Recorder|
| 127|B.A.S.E. Jumper|
| 128|Half-Zatoichi|
| 129|Pain Train|
| 130|Panic Attack|
| 131|Reserve Shooter|
| 132|Shotgun|
| 133|Mad Milk|
| 134|Mutated Milk|
| 135|Bat|
| 136|Batsaber|
| 137|Boston Basher|
| 138|Holy Mackarel|
| 139|Three-Rune Blade|
| 140|Unarmed Combat|
| 141|Original|
| 142|Rocket Launcher|
| 143|Flame Thrower|
| 144|Nostromo Napalmer|
| 145|Axtinguisher|
| 146|Homewrecker|
| 147|Maul|
| 148|Postal Pummeler|
| 149|Ali Baba's Wee Booties|
| 150|Bootlegger|
| 151|Bottle|
| 152|Eyelander|
| 153|Horseless Headless Horsemann's Headtaker|
| 154|Nessie's Nine Iron|
| 155|Scottish Handshake|
| 156|Iron Curtain|
| 157|Minigun|
| 158|Dalokohs Bar|
| 159|Fishcake|
| 160|Robo-Sandvich|
| 161|Sandvich|
| 162|Apoco-Fists|
| 163|Bread Bite|
| 164|Fists|
| 165|Gloves of Running Urgently|
| 166|Giger Counter|
| 167|Wrangler|
| 168|Wrench|
| 169|AWPer Hand|
| 170|Fortified Compound|
| 171|Huntsman|
| 172|Machina|
| 173|Shooting Star|
| 174|Sniper Rifle|
| 175|Jarate|
| 176|Self-Aware Beauty Mark|
| 177|Big Kill|
| 178|Revolver|
| 179|Enthusiast's Timepiece|
| 180|Invis Watch|
| 181|Quäckenbirdt|
| 182|Black Rose|
| 183|Knife|
| 184|Sharp Dresser|
| 185|Wanga Prick|
| 186|Your Eternal Reward|
| 187|Sapper|
| 188|Ap-Sap|
| 189|Snack Attack|
| 190|Bat Outta Hell|
| 191|C.A.P.P.E.R|
| 192|Conscientious Objector|
| 193|Crossing Guard|
| 194|Freedom Staff|
| 195|Frying Pan|
| 196|Ham Shank|
| 197|Necro Smasher|
| 198|Lugermorph|
| 199|Pistol|
| 200|Prinny Machete|
