using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(LSUnitView))]
	public class LSViewGridBuilderComponent : Entity, IAwake, IDestroy
	{
		public Placement DragPlacement;
		public EntityRef<LSUnitView> DragUnitView;
		public Vector3 DragOffset;
	}
}