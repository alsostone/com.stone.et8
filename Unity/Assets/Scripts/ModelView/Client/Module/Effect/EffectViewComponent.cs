using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf]
	public class EffectViewComponent : Entity, IAwake, IDestroy
	{
		public Dictionary<int, EntityRef<EffectView>> Effects = new ();
	}
}