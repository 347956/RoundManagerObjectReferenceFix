using HarmonyLib;
using System.Linq;

namespace RoundManagerObjectReferenceFix.src.Patches
{
    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SpawnScrapInLevel))]
    public class SpawnScrapPatch
    {
        [HarmonyPrefix]
        static void Prefix(RoundManager __instance)
        {
            // Get valid items from the moon's own spawn list
            var validItems = __instance.currentLevel.spawnableScrap
                .Where(s => s.spawnableItem?.spawnPrefab != null)
                .Select(s => s.spawnableItem)
                .ToList();

            if (validItems.Count == 0)
            {
                Plugin.Logger.LogWarning("No valid items found in moon's spawn list, cannot replace null prefabs!");

                return;
            }

            foreach (var spawnableScrap in __instance.currentLevel.spawnableScrap)
            {
                if (spawnableScrap == null)
                {
                    Plugin.Logger.LogWarning("Null scrap entry in spawn list");

                    continue;
                }

                var item = spawnableScrap.spawnableItem;

                if (item?.spawnPrefab == null)
                {
                    var replacement = validItems[UnityEngine.Random.Range(0, validItems.Count)];

                    Plugin.Logger.LogWarning($"Item '{item?.name ?? "null"}' has null prefab, replacing with '{replacement!.name}'");

                    spawnableScrap.spawnableItem = replacement;
                }
            }
        }
    }
}
