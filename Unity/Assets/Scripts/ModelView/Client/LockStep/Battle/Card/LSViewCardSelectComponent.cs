using System;
using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf(typeof(LSUnitView))]
	public class LSViewCardSelectComponent : Entity
	{
		public List<List<Tuple<EUnitType, int, int>>> CardsQueue { get; } = new ();
	}
}