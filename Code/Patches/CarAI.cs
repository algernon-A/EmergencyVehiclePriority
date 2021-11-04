using HarmonyLib;


namespace EmergencyVehiclePriority
{
	/// <summary>
	/// Harmony patch to selectively disable collision checking with other vehicles.
	/// </summary>
	[HarmonyPatch(typeof(CarAI), "CheckOtherVehicles")]
	public static class CheckVehiclesPatch
	{
		/// <summary>
		/// Harmony Prefix patch to disable collision checking between cars for emergency vehicles on call.
		/// </summary>
		/// <param name="vehicleData">Vehicle data</param>
		/// <returns>False to pre-empt original method (abort execution chain), true otherwise</returns>
		public static bool Prefix(ref Vehicle vehicleData)
		{
			// Check to see if this vehicle is currently on emergency response.
			if ((vehicleData.m_flags & Vehicle.Flags.Emergency2) != 0)
			{
				// Yes; simply return false to finish execution chain (bypassing collision detection).
				return false;
			}

			// If we got here, then normal vehicle behaviour should be observed; return true to continue original method execution.
			return true;
		}
	}


	/// <summary>
	/// Harmony patch to selectively disable collision checking with pedestrians.
	/// </summary>
	[HarmonyPatch(typeof(CarAI), "CheckCitizen")]
	public static class CheckCitizen
	{
		/// <summary>
		/// Harmony Prefix patch to disable collision checking with pedestrians for emergency vehicles on call.
		/// </summary>
		/// <param name="__result">Original method result</param>
		/// <param name="vehicleData">Vehicle data</param>
		/// <param name="otherData">Citizen data for this citizen</param>
		/// <returns>False to pre-empt original method (abort execution chain), true otherwise</returns>
		public static bool Prefix(ref ushort __result, ref Vehicle vehicleData, ref CitizenInstance otherData)
		{
			// Check to see if this vehicle is currently on emergency response.
			if ((vehicleData.m_flags & Vehicle.Flags.Emergency2) != 0)
			{
				// Yes; set method result to the next citizen instance in this grid (as per game method) and return false to finish execution chain (bypassing collision detection).
				__result = otherData.m_nextGridInstance;
				return false;
			}

			// If we got here, then normal vehicle behaviour should be observed; return true to continue original method execution.
			return true;
		}
	}
}