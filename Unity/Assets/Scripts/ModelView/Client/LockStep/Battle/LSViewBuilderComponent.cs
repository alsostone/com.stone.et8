using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewBuilderComponent : Entity, IAwake, IDestroy
	{
		public Placement DragPlacement;
		public Vector3 DragOffset;
	}
}