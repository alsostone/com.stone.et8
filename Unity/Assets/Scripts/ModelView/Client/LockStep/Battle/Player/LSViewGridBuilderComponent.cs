using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(LSUnitView))]
	public class LSViewGridBuilderComponent : Entity, IAwake, IDestroy, ILSRollback
	{
		public Placement DragPlacement;
		public TbItemRow DragItemRow;
		public EntityRef<LSUnitView> DragUnitView;
		public Vector3 DragOffset;
		
		public bool IsDragging;
		public Vector3 DragStartPosition;
		public Vector3 DragPosition;
		public bool IsSelectDragging;
	}
}