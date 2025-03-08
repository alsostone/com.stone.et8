using System.Collections.Generic;


namespace ET.Server
{
	public static partial class RealmGateAddressHelper
	{
		public static TbStartSceneRow GetGate(int zone, string account)
		{
			ulong hash = (ulong)account.GetLongHashCode();
			
			List<TbStartSceneRow> zoneGates = TbStartScene.Instance.Gates[zone];
			
			return zoneGates[(int)(hash % (ulong)zoneGates.Count)];
		}
	}
}
