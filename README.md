# RoundManagerObjectReferenceFix

>ℹ️ Only the host is required to install this mod. Should be fine for multiplayer and could potentially prevent crashes on other custom moons too.

A mod for Lethal Company that prevents crashes caused by unresolved prefab references in custom moon mods.

## What it does

>ℹ️ I created this mod for my own moon as I assigned blank references to the corresponding fields (to keep the mod size small), but the game could not resolve them and crash.


This mod runs during the spawn process of the RoundManager and tries to detect scrap, outside objects, and map hazards that do not have a prefab ( prefab = null).
It does the following when a `prefab = null`:

- **Scrap items**:
	- If an item has a null prefab, it is replaced with a valid item from the moon's item spawn list that has a similar value (one of the 3 closest). If the item has no value range to compare, a random valid item is used instead
- **Outside objects**:
	- If an outside object has a null prefab, the prefab is resolved from a vanilla level's object with the same name. If no match exists, it is removed from the spawn pool to prevent a crash
- **Map objects**:
	- If an Indoor hazard has a null prefab, the prefab is resolved from a vanilla level's hazard with the same name. If no match exists, it is removed from the spawn pool to prevent a crash

Logging is included so you can identify which objects are not resolving correctly and fix them in your moon configuration.


## Example errors
### On spawn outside hazards:
```log
[Error  : Unity Log] ArgumentException: The Object you want to instantiate is null.
Stack trace:
UnityEngine.Object.Instantiate (UnityEngine.Object original, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation, UnityEngine.Transform parent) (at <c39a522eee05469b8171a6cfeb646c59>:IL_0048)
UnityEngine.Object.Instantiate[T] (T original, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation, UnityEngine.Transform parent) (at <c39a522eee05469b8171a6cfeb646c59>:IL_0001)
RoundManager.SpawnOutsideHazards () (at <aca1e98d6f844d3f85cd22b6f7be2f9b>:IL_049F)
DMD<>?434046712.Trampoline<RoundManager::SpawnOutsideHazards>?-426100634 (RoundManager ) (at <6ab606ea172a41089b084a0fac1d3b4f>:IL_0025)
Dawn.MapObjectRegistrationHandler.SpawnOutsideMapObjects (On.RoundManager+orig_SpawnOutsideHazards orig, RoundManager self) (at ./src/API/MapObjects/.MapObjectRegistrationHandler.cs:275)
DMD<>?1224120700.Hook<RoundManager::SpawnOutsideHazards>?1737978544 (RoundManager ) (at <08e77660bbec49c79358861a9942a2e8>:IL_000F)
RoundManager.FinishGeneratingLevel () (at <aca1e98d6f844d3f85cd22b6f7be2f9b>:IL_0040)
RoundManager.Generator_OnGenerationStatusChanged (DunGen.DungeonGenerator generator, DunGen.GenerationStatus status) (at <aca1e98d6f844d3f85cd22b6f7be2f9b>:IL_000C)
DunGen.DungeonGenerator.ChangeStatus (DunGen.GenerationStatus status) (at <761948217ee64e61867b0efbfe186c99>:IL_0036)
DunGen.DungeonGenerator+<InnerGenerate>d__131.MoveNext () (at <761948217ee64e61867b0efbfe186c99>:IL_049E)
UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at <c39a522eee05469b8171a6cfeb646c59>:IL_0026)
```

### On spawning scrap
```log
[Error  : Unity Log] ArgumentException: The Object you want to instantiate is null.
Stack trace:
UnityEngine.Object.Instantiate (UnityEngine.Object original, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation) (at <c39a522eee05469b8171a6cfeb646c59>:IL_0049)
UnityEngine.Object.Instantiate (UnityEngine.Object original, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation, UnityEngine.Transform parent) (at <c39a522eee05469b8171a6cfeb646c59>:IL_000C)
UnityEngine.Object.Instantiate[T] (T original, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation, UnityEngine.Transform parent) (at <c39a522eee05469b8171a6cfeb646c59>:IL_0001)
RoundManager.SpawnScrapInLevel () (at <aca1e98d6f844d3f85cd22b6f7be2f9b>:IL_0540)
DMD<>?2052090776.Trampoline<RoundManager::SpawnScrapInLevel>?1603470438 (RoundManager ) (at <ba2f074d161c45b7b28b375f042274b3>:IL_0025)
Dawn.ItemRegistrationHandler.UpdateItemWeights (On.RoundManager+orig_SpawnScrapInLevel orig, RoundManager self) (at ./src/API/Items/.ItemRegistrationHandler.cs:107)
DMD<>?1148133988.Hook<RoundManager::SpawnScrapInLevel>?1662684564 (RoundManager ) (at <0fc5dc6ba0e8494d96b22eb435ae30a3>:IL_000F)
RoundManager.GeneratedFloorPostProcessing () (at <aca1e98d6f844d3f85cd22b6f7be2f9b>:IL_0009)
RoundManager+<LoadNewLevelWait>d__167.MoveNext () (at <aca1e98d6f844d3f85cd22b6f7be2f9b>:IL_05C5)
UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at <c39a522eee05469b8171a6cfeb646c59>:IL_0026)
```

## Example of the fix
### Outside "hazards" / objects:
```log
[Info   :RoundManagerObjectReferenceFix] Checking: SpawnOutsideHazards
[Info   :RoundManagerObjectReferenceFix] Outside object 'SnowmanShort' | Prefab: Snowman OK
[Info   :RoundManagerObjectReferenceFix] Outside object 'SnowmanTall' | Prefab: SnowmanTall OK
[Warning:RoundManagerObjectReferenceFix] Outside object 'TreeLeafless2Snowy' has null prefab, resolving from vanilla level
[Warning:RoundManagerObjectReferenceFix] Outside object 'SmallGreyRocks2' has null prefab, resolving from vanilla level
[Warning:RoundManagerObjectReferenceFix] Outside object 'SmallGreyRocks1' has null prefab, resolving from vanilla level
[Warning:RoundManagerObjectReferenceFix] Outside object 'TreeLeafless3Snowy' has null prefab, resolving from vanilla level
```

### scrap:
```log
[Info   : Unity Log] Dungeon has finished generating on this client after multiple frames
[Info   : Unity Log] Players finished generating the new floor
[Info   : Unity Log] Spawning synced props on server. Length: 39
[Info   :RoundManagerObjectReferenceFix] Checking: SpawnScrapInLevel
[Warning:RoundManagerObjectReferenceFix] Scrap item 'CardboardBox' has null prefab, replacing with 'Flask'
[Info   : Unity Log] Number of scrap to spawn: 24. minTotalScrapValue: 500. Total value of items: 0.
```

### MapObjects "indoor hazards":
```log
[Info   :RoundManagerObjectReferenceFix] Checking: SpawnMapObjects
[Warning:RoundManagerObjectReferenceFix] Indoor hazard 'Turrets' has null prefab, resolving from vanilla level
[Warning:RoundManagerObjectReferenceFix] Indoor hazard 'SpikeTraps' has null prefab, resolving from vanilla level
[Warning:RoundManagerObjectReferenceFix] Indoor hazard 'Landmines' has null prefab, resolving from vanilla level
[Info   : Unity Log] TurretScript creating doorcode object in parent MapScreenUIWorldSpace (UnityEngine.RectTransform): DoorCodeUIObject(Clone)
[Info   : Unity Log] TerminalObjectContainer creating doorcode object in parent MapScreenUIWorldSpace (UnityEngine.RectTransform): DoorCodeUIObject(Clone)
[Info   : Unity Log] Landmine creating doorcode object in parent MapScreenUIWorldSpace (UnityEngine.RectTransform): DoorCodeUIObject(Clone)
```