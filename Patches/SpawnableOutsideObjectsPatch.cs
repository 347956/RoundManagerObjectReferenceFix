using HarmonyLib;
using System.Linq;

namespace RoundManagerObjectReferenceFix.Patches
{
    // SpawnOutsideHazards is a private method, so we cannot use nameof() here
    [HarmonyPatch(typeof(RoundManager), "SpawnOutsideHazards")]
    public class SpawnableOutsideObjectsPatch
    {
        [HarmonyPrefix]
        static void Prefix(RoundManager __instance)
        {
            // A failing safety net must never break the spawn it protects
            try
            {
                Check(__instance);
            }
            catch (System.Exception e)
            {
                Plugin.Logger.LogError($"Outside object check failed, continuing with vanilla behaviour: {e}");
            }
        }

        static void Check(RoundManager __instance)
        {
            Plugin.Logger.LogInfo($"Checking: SpawnOutsideHazards");

            if (__instance.currentLevel.spawnableOutsideObjects == null) return;

            var level = __instance.currentLevel;

            for (int index = 0; index < level.spawnableOutsideObjects.Length; index++)
            {
                var outsideObj = level.spawnableOutsideObjects[index];

                if (outsideObj == null)
                {
                    if (Plugin.FirstTime($"outside-null-entry:{level.name}:{index}"))
                        Plugin.Logger.LogWarning($"Null outside object entry at index {index} in spawn list of '{level.name}'");
                    continue;
                }

                var spawnAbleObject = outsideObj.spawnableObject;

                if (spawnAbleObject == null)
                {
                    if (Plugin.FirstTime($"outside-null-object:{level.name}:{index}"))
                        Plugin.Logger.LogWarning($"Outside object at index {index} in '{level.name}' has null spawnableObject, removing from spawn list");

                    outsideObj.randomAmount = new UnityEngine.AnimationCurve();

                    continue;
                }

                if (spawnAbleObject.prefabToSpawn != null)
                {
                    if (Plugin.FirstTime($"outside-ok:{spawnAbleObject.name}"))
                        Plugin.Logger.LogInfo($"Outside object '{spawnAbleObject.name}' | Prefab: {spawnAbleObject.prefabToSpawn.name} OK");

                    continue;
                }

                var match = StartOfRound.Instance.levels
                    .Where(l => l?.spawnableOutsideObjects != null)
                    .SelectMany(l => l.spawnableOutsideObjects)
                    .FirstOrDefault(o =>
                        o?.spawnableObject?.prefabToSpawn != null &&
                        o.spawnableObject.name == spawnAbleObject.name)
                    ?.spawnableObject;

                if (match != null)
                {
                    Plugin.Logger.LogWarning($"Outside object '{spawnAbleObject.name}' has null prefab, resolving from vanilla level");

                    spawnAbleObject.prefabToSpawn = match.prefabToSpawn;
                }
                else
                {
                    if (Plugin.FirstTime($"outside-removed:{spawnAbleObject.name}"))
                        Plugin.Logger.LogWarning($"Outside object '{spawnAbleObject.name}' has null prefab, removing from spawn list");

                    outsideObj.randomAmount = new UnityEngine.AnimationCurve();
                }
            }
        }
    }
}
