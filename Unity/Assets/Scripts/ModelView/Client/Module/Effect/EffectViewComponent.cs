using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf]
	public class EffectViewComponent : Entity, IAwake, IDestroy
	{
		public long idGenerator;
		public Dictionary<int, EntityRef<EffectView>> effectViews = new ();
	}
}