using System;
using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf(typeof(LSUnitView))]
	public class LSViewCardBagComponent : Entity, IAwake, ILSRollback
	{
		public Dictionary<(EUnitType, int), int> ItemCountMap { get; } = new();
	}
}