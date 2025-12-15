using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	[EntitySystemOf(typeof(LSSettingsComponent))]
	[FriendOf(typeof(LSSettingsComponent))]
	public static partial class LSSettingsComponentSystem
	{
		[EntitySystem]
		private static void Awake(this LSSettingsComponent self)
		{
			self.HudColor = new Dictionary<TeamType, Color>(4);
			self.HudColor[TeamType.None] = new Color(0.8f, 0.8f, 0.8f, 1f);
			self.HudColor[TeamType.TeamA] = new Color(0.2f, 0.8f, 0.3f, 1f);
			self.HudColor[TeamType.TeamB] = new Color(0.8f, 0.1f, 0.1f, 1f);
			self.HudColor[TeamType.TeamNeutral] = new Color(0.8f, 0.8f, 0.8f, 1f);
		}
		
		public static Color GetHudColor(this LSSettingsComponent self, LSUnitView lsUnitView)
		{
			LSUnit lsOwner = lsUnitView.GetUnit();
			TeamType teamType = lsOwner.GetComponent<TeamComponent>().Type;
			return self.HudColor[teamType];
		}
	}
}
