# Changelog

## 1.0.1

- Scrap items with a null prefab are now replaced by a scrap item of similar value from the moon's spawn list. If the item has no value range to compare, a random valid item is used as before.
- Outside objects and indoor hazards with a null prefab are first resolved from a vanilla level's object with the same name. They are only removed from the spawn pool when no match exists.
- The mod can no longer cause a crash itself: if one of its checks fails, the error is logged and the game continues as it would without the mod.
- Fixed possible errors when a moon has missing scrap, hazard or outside object lists, or when another level in the game has them missing.
- Repeated log messages are now logged once per session instead of on every level load.
- Warnings for outside object entries without a `spawnableObject` now show the entry index and the moon name, so the broken entry is easy to find.

## 1.0.0

- Initial release.
- Replaces scrap with a null prefab with a valid item from the moon's spawn list.
- Removes outside objects and indoor hazards with a null prefab from the spawn pool to prevent a crash.
