using HarmonyLib;
using System.Linq;

namespace RoundManagerObjectReferenceFix.src.Patches
{
    // SpawnOutsideHazards is a private method, so we cannot use nameof() here
    [HarmonyPatch(typeof(RoundManager), "SpawnOutsideHazards")]
    public class SpawnableOutsideObjectsPatch
    {
        [HarmonyPrefix]
        static void Prefix(RoundManager __instance)
        {
            Plugin.Logger.LogInfo($"Checking: SpawnOutsideHazards");

            if (__instance.currentLevel.spawnableOutsideObjects == null) return;

            foreach (var outsideObj in __instance.currentLevel.spawnableOutsideObjects)
            {
                if (outsideObj == null)
                {
                    Plugin.Logger.LogWarning("Null outside object entry in spawn list");
                    continue;
                }

                var spawnAbleObject = outsideObj.spawnableObject;

                if (spawnAbleObject == null)
                {
                    Plugin.Logger.LogWarning("Outside object has null spawnableObject, removing from spawn list");

                    outsideObj.randomAmount = new UnityEngine.AnimationCurve();

                    continue;
                }

                if (spawnAbleObject.prefabToSpawn != null)
                {
                    Plugin.Logger.LogInfo($"Outside object '{spawnAbleObject.name}' | Prefab: {spawnAbleObject.prefabToSpawn.name} OK");

                    continue;
                }

                var match = StartOfRound.Instance.levels
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
                    Plugin.Logger.LogWarning($"Outside object '{spawnAbleObject.name}' has null prefab, removing from spawn list");

                    outsideObj.randomAmount = new UnityEngine.AnimationCurve();
                }
            }
        }
    }
}
