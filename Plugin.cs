using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace RoundManagerObjectReferenceFix
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = null!;

        private static readonly HashSet<string> Logged = new HashSet<string>();

        // True only the first time a key is seen, keeps per-level-load logs from repeating
        internal static bool FirstTime(string key) => Logged.Add(key);

        private void Awake()
        {
            Logger = base.Logger;

            new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();

            Logger.LogInfo($"Plugin [{PluginInfo.PLUGIN_GUID} version: {PluginInfo.PLUGIN_VERSION}] is loaded!");
        }
    }
}
