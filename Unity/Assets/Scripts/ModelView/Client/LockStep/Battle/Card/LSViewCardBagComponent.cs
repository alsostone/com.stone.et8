using System;
using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf(typeof(LSUnitView))]
	public class LSViewCardBagComponent : Entity, IAwake, ILSRollback
	{
		public List<CardBagItem> Items { get; } = new();
		public Dictionary<long, CardBagItem> IdItemMap { get; } = new();
	}
}