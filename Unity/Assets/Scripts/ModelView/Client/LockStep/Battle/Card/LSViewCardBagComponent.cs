using System;
using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf(typeof(LSUnitView))]
	public class LSViewCardBagComponent : Entity, IAwake, ILSRollback
	{
		public List<Tuple<EUnitType, int, int>> Items { get; } = new ();
	}
}