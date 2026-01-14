using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf]
	public class ViewEffectComponent : Entity, IAwake<float>, IDestroy
	{
		public long idGenerator;
		
		public float Speed;
		public Dictionary<int, EntityRef<ViewEffect>> SkillEffectViews = new ();
		public Dictionary<int, EntityRef<ViewEffect>> EffectViews = new ();
	}
}