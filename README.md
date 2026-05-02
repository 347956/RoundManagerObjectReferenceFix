# RoundManagerObjectReferenceFix

>ℹ️ Only the host is required to install this mod. Should be fine for multiplayer and could potentially prevent crashes on other custom moons too.

A mod for Lethal Company that prevents crashes caused by unresolved prefab references in custom moon mods.

## What it does

>I created this mod for my own moon as I assigned blank references to the corresponding fields (to keep the mod size small), but the game could not resolve them and crash.


This mod runs during the spawn process of the RoundManager and tries to detect scrap, outside objects, and map hazards that do not have a prefab ( prefab = null).
It does the following when a `prefab = null`:

- **Scrap items**:
	- If an item has a null prefab, it is replaced with a random valid item from the moon's item spawn list
- **Outside objects**:
	- If an outside object has a null prefab, it is removed from the spawn pool to prevent a crash
- **Map objects**:
	- If an Indoor hazard has a null prefab, it is removed from the spawn pool to prevent a crash

Logging is included so you can identify which objects are not resolving correctly and fix them in your moon configuration.