using System;
using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf(typeof(LSUnitView))]
	public class LSViewCardSelectComponent : Entity, IAwake, ILSRollback
	{
		public List<List<Tuple<EUnitType, int, int>>> CardsQueue { get; } = new ();
	}
}