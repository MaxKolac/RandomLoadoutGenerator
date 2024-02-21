## Overview

A C# library that allows you to choose random weapons from all weapons available in Team Fortress 2.
You can specify for what class and for what slot should a weapon be randomized.

This library supports weapons that are multi-class and are equippable in different slots, for example The Panic Attack or The B.A.S.E. Jumper.

It's also possible to tell the `Generator` to treat weapon reskins as a single weapon to increase the odds of rolling mechanically different unlockable weapons.
This way you can minimize the odds of pulling different melee reskins 5 times in a row.

## Boring details

It uses a small built-in SQLite packaged as an embedded resource. 
When you create a `Generator` instance, it unpacks this database into the `Environment.CurrentDirectory` (usually the same directory where your app's executable is).

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

You can also enable `Generator` to treat weapon reskins as a single weapon and have higher chances of returning a mechanically unique weapon by adding `true` as last 3rd argument.

```
var weapon = generator.RandomizeWeapon(TFClass.Scout, TFSlot.Melee, true);
```

You can also take a look at `examples` folder on the GitHub repository for an example project. It's a simple console app that generates a new random loadout for a random class everytime you press Enter.
