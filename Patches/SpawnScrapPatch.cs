using HarmonyLib;
using System.Linq;

namespace RoundManagerObjectReferenceFix.Patches
{
    [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.SpawnScrapInLevel))]
    public class SpawnScrapPatch
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
                Plugin.Logger.LogError($"Scrap check failed, continuing with vanilla behaviour: {e}");
            }
        }

        // How many of the closest-valued items are candidates, to keep some variety
        const int ClosestCandidates = 3;

        // Prefers an item with a similar value range, falls back to a random valid item
        static Item PickReplacement(Item broken, System.Collections.Generic.List<Item> validItems)
        {
            // Without a usable value range there is nothing to compare against
            if (broken.minValue <= 0 && broken.maxValue <= 0)
                return validItems[UnityEngine.Random.Range(0, validItems.Count)];

            float brokenValue = (broken.minValue + broken.maxValue) / 2f;

            var closest = validItems
                .OrderBy(i => System.Math.Abs((i.minValue + i.maxValue) / 2f - brokenValue))
                .Take(ClosestCandidates)
                .ToList();

            return closest[UnityEngine.Random.Range(0, closest.Count)];
        }

        static void Check(RoundManager __instance)
        {
            Plugin.Logger.LogInfo($"Checking: {nameof(RoundManager.SpawnScrapInLevel)}");

            if (__instance.currentLevel.spawnableScrap == null) return;

            // Get valid items from the moon's own spawn list
            var validItems = __instance.currentLevel.spawnableScrap
                .Where(s => s?.spawnableItem?.spawnPrefab != null)
                .Select(s => s.spawnableItem)
                .ToList();

            if (validItems.Count == 0)
            {
                Plugin.Logger.LogWarning("No valid items found in moon's spawn list, cannot replace null prefabs!");

                return;
            }

            bool allOk = true;

            foreach (var spawnableScrap in __instance.currentLevel.spawnableScrap)
            {
                if (spawnableScrap == null)
                {
                    Plugin.Logger.LogWarning("Null scrap entry in spawn list");

                    allOk = false;

                    continue;
                }

                var item = spawnableScrap.spawnableItem;

                if (item == null)
                {
                    Plugin.Logger.LogWarning("Scrap entry has null spawnableItem, skipping");

                    allOk = false;

                    continue;
                }

                if (item.spawnPrefab != null) continue;

                allOk = false;

                var replacement = PickReplacement(item, validItems);

                Plugin.Logger.LogWarning($"Scrap item '{item.name}' has null prefab, replacing with '{replacement!.name}'");

                spawnableScrap.spawnableItem = replacement;
            }

            if (allOk)
                Plugin.Logger.LogInfo("All scrap items OK");
        }
    }
}
