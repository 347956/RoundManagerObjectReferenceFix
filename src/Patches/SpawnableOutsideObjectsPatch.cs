using HarmonyLib;

namespace RoundManagerObjectReferenceFix.src.Patches
{
    // SpawnOutsideHazards is a private method, so we cannot use nameof() here
    [HarmonyPatch(typeof(RoundManager), "SpawnOutsideHazards")]
    public class SpawnableOutsideObjectsPatch
    {
        [HarmonyPrefix]
        static void Prefix(RoundManager __instance)
        {
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

                if (spawnAbleObject.prefabToSpawn == null)
                {
                    Plugin.Logger.LogWarning($"Outside object '{spawnAbleObject.name}' has null prefab, removing from spawn list");

                    outsideObj.randomAmount = new UnityEngine.AnimationCurve();
                }
            }
        }
    }
}
