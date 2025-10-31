using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
	[ComponentOf(typeof(Room))]
	public class LSCameraComponent : Entity, IAwake, ILateUpdate
	{
		// 战斗摄像机
		private Camera camera;

		public Transform Transform;
		
		public Camera Camera
		{
			get
			{
				return this.camera;
			}
			set
			{
				this.camera = value;
				this.Transform = this.camera.transform;
			}
		}

		private EntityRef<LSUnitView> lookUnitView;

		public LSUnitView LookUnitView
		{
			get
			{
				return this.lookUnitView;
			}
			set
			{
				this.lookUnitView = value;
			}
		}

		public int LookSeatIndex;
		
		public bool IsFlowTarget = false;
		public Vector3 LookPosition;
		
		public Vector3 DragMousePosition;
		public bool IsDragging = false;
		public Plane DragGroundPlane = new Plane(Vector3.up, Vector3.zero);
		public float DragSpeedRatio = 0.5f;
	}
}
