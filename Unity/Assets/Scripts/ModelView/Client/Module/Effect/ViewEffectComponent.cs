using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf]
	public class ViewEffectComponent : Entity, IAwake, IDestroy
	{
		public long idGenerator;
		public Dictionary<int, EntityRef<EffectView>> SkillEffectViews = new ();
		public Dictionary<int, EntityRef<EffectView>> EffectViews = new ();
	}
}