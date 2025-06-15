using UnityEngine;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewSkillComponent : Entity, IAwake, IDestroy, ILSRollback
	{
		public long AniPreTimer;
		public long AniTimer;
		public long AniAfterTimer;
		
		public bool IsSkillRunning;
		public TbSkillRow TbSkillRow;
	}
}