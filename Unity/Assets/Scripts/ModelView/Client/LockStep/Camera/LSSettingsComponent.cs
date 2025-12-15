using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(Room))]
	public class LSSettingsComponent : Entity, IAwake
	{
		public Dictionary<TeamType, Color> HudColor;
	}

}
