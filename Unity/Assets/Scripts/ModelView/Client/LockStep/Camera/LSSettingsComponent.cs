using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(Room))]
	public class LSSettingsComponent : Entity, IAwake
	{
		public Color[] HudColor;
	}

}
