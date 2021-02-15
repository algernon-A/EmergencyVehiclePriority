using CitiesHarmony.API;
using ICities;

namespace EmergencyVehiclePriority
{
	/// <summary>
	/// The base mod class for instantiation by the game.
	/// </summary>
	public class EVPMod : IUserMod
	{
		public static string ModName => "Emergency Vehicle Priority";
		public static string Version => "0.0.1";
		public string Name => ModName + " " + Version;
		public string Description => "Emergency vehicles on call pass through other traffic.";


        /// <summary>
        /// Called by the game when the mod is enabled.
        /// </summary>
        public void OnEnabled()
        {
            // Apply Harmony patches via Cities Harmony.
            // Called here instead of OnCreated to allow the auto-downloader to do its work prior to launch.
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }


        /// <summary>
        /// Called by the game when the mod is disabled.
        /// </summary>
        public void OnDisabled()
        {
            // Unapply Harmony patches via Cities Harmony.
            if (HarmonyHelper.IsHarmonyInstalled)
            {
                Patcher.UnpatchAll();
            }
        }
    }
}
