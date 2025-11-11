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
			self.HudColor = new Color[(int)TeamType.Max];
			self.HudColor[(int)TeamType.None] = new Color(0.8f, 0.8f, 0.8f, 1f);
			self.HudColor[(int)TeamType.TeamA] = new Color(0.2f, 0.8f, 0.3f, 1f);
			self.HudColor[(int)TeamType.TeamB] = new Color(0.8f, 0.1f, 0.1f, 1f);
		}
		
		public static Color GetHudColor(this LSSettingsComponent self, LSUnitView lsUnitView)
		{
			LSUnit lsOwner = lsUnitView.GetUnit();
			TeamType teamType = lsOwner.GetComponent<TeamComponent>().Type;
			return self.HudColor[(int)teamType];
		}
	}
}
