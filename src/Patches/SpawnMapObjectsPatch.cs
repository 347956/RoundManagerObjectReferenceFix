using HarmonyLib;
using System.Linq;

namespace RoundManagerObjectReferenceFix.src.Patches
{
    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SpawnMapObjects))]
    public class SpawnMapObjectsPatch
    {
        [HarmonyPrefix]
        static void Prefix(RoundManager __instance)
        {
            Plugin.Logger.LogInfo($"Checking: {nameof(RoundManager.SpawnMapObjects)}");

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

                // Prefab is valid, continue to next
                if (hazardType.prefabToSpawn != null)
                {
                    Plugin.Logger.LogInfo($"Indoor Hazard: '{hazardType.name}' | Prefab: {hazardType.prefabToSpawn.name} OK");

                    continue;
                }

                // Try to find a matching hazard by name from any vanilla level
                IndoorMapHazardType? match = null;
                foreach (var level in StartOfRound.Instance.levels)
                {
                    match = level.indoorMapHazards
                        .FirstOrDefault(h =>
                            h?.hazardType?.prefabToSpawn != null &&
                            h.hazardType.name == hazardType.name)
                        ?.hazardType;

                    if (match != null) break;
                }

                if (match != null)
                {
                    Plugin.Logger.LogWarning($"Indoor hazard '{hazardType.name}' has null prefab, resolving from vanilla level");

                    hazardType.prefabToSpawn = match.prefabToSpawn;
                }
                else
                {
                    Plugin.Logger.LogWarning($"Indoor hazard '{hazardType.name}' has null prefab, removing from spawn list");

                    hazard.numberToSpawn = new UnityEngine.AnimationCurve();
                }
            }
        }
    }
}
