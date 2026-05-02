using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace RoundManagerObjectReferenceFix
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = null!;

        private void Awake()
        {
            Logger = base.Logger;

            new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();

            Logger.LogInfo($"Plugin [{PluginInfo.PLUGIN_GUID} version: {PluginInfo.PLUGIN_VERSION}] is loaded!");
        }
    }
}
