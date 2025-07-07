using System;
using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
	[ComponentOf]
	public class LSGridBuilderComponent : Entity, IAwake, IDestroy
	{
		public Placement dragPlacement;
		
		public Vector3 dragOffset;
		public PlacementData dragPlacementData => dragPlacement.placementData;
		
		public WeakReference<GridMap> gridMapReference;
	}
}