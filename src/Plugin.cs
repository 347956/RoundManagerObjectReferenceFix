using BepInEx;
using BepInEx.Logging;
using Dawn;
using HarmonyLib;

namespace RoundManagerObjectReferenceFix.src
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency(DawnLib.PLUGIN_GUID)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger = null!;

        private void Awake()
        {
            Logger = base.Logger;
            new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();
            Logger.LogInfo($"Plugin [{PluginInfo.PLUGIN_GUID}] is loaded!");
        }
    }
}
