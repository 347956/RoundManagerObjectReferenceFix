# RoundManagerObjectReferenceFix

>ℹ️ Only the host is required to install this mod. Should be fine for multiplayer and could potentially prevent crashes on other custom moons too.

A simple mod for Lethal Company that tries to prevent crashes caused by unresolved prefab references in custom moon mods built with DawnLib.

## What it does

I created this mod for my own moon as I assigned blank references to the corresponding fields, but the game could not resolve them and crash.

Logging is also included so you can check which objects are not assigned correctly, and hopefully fix them.

This mod runs during the spawn proces of the RoundManager and tries to detect scrap, outside objects, and map hazards that do not have a prefab (`prefab = null`) and the replace them with an available vanilla asset:

- **Scrap items** 
	- if an item has a null prefab, it is replaced with a random valid item from the moon's own item spawn list
- **Outside objects & Map hazards** 
	- if an object has a null prefab, it first tries to resolve it by finding a matching object from any vanilla level. If no match is found, it is removed from the spawn pool to prevent a crash