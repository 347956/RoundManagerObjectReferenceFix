using HarmonyLib;

namespace RoundManagerObjectReferenceFix.src.Patches
{
    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SpawnMapObjects))]
    public class SpawnMapObjectsPatch
    {
        [HarmonyPrefix]
        static void Prefix(RoundManager __instance)
        {
            if (__instance.currentLevel.indoorMapHazards == null) return;

            foreach (var hazard in __instance.currentLevel.indoorMapHazards)
            {
                if (hazard == null)
                {
                    Plugin.Logger.LogWarning("Null indoor hazard entry in spawn list");

                    continue;
                }

                var hazardType = hazard.hazardType;

                if (hazardType == null)
                {
                    Plugin.Logger.LogWarning("Null hazard type entry in spawn list");

                    continue;
                }

                if (hazardType.prefabToSpawn == null)
                {
                    Plugin.Logger.LogWarning($"Indoor hazard '{hazardType.name}' has null prefab, removing from spawn list");

                    hazard.numberToSpawn = new UnityEngine.AnimationCurve();
                }
                else
                {
                    Plugin.Logger.LogInfo($"Indoor Hazard: '{hazardType.name}' | Prefab: {hazardType.prefabToSpawn.name} OK");
                }
            }
        }
    }
}
