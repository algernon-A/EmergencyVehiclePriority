using UnityEngine;
using ColossalFramework.Math;
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
		/// <param name="vehicleID">Vehicle ID (ignored)</param>
		/// <param name="vehicleData">Vehicle data</param>
		/// <param name="frameData">Frame data (ignored)</param>
		/// <param name="maxSpeed">Vehicle maximum speed (ignored)</param>
		/// <param name="blocked">Vehicle blocked flag (ignored)</param>
		/// <param name="collisionPush">Collision push vector (ignored)</param>
		/// <param name="maxDistance">Maximum distance (ignored)</param>
		/// <param name="maxBraking">Vehicle maximum breaking (ignored)</param>
		/// <param name="lodPhysics">Physics LOD level (ignored)</param>
		/// <returns>False to pre-empt original method (abort execution chain), true otherwise</returns>
		public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref Vehicle.Frame frameData, ref float maxSpeed, ref bool blocked, ref Vector3 collisionPush, float maxDistance, float maxBraking, int lodPhysics)
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
	/// Harmony patch to selectively disable pedestrian collision chicking.
	/// </summary>
	[HarmonyPatch(typeof(CarAI), "CheckCitizen")]
	public static class CheckCitizen
	{

		public static bool Prefix(ref ushort __result, ushort vehicleID, ref Vehicle vehicleData, Segment3 segment, float lastLen, float nextLen, ref float maxSpeed, ref bool blocked, float maxBraking, ushort otherID, ref CitizenInstance otherData, Vector3 min, Vector3 max)
		{
			__result = otherData.m_nextGridInstance;

			return false;
		}
	}
}