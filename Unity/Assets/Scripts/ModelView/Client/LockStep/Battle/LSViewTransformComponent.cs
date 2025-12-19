
using UnityEngine;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewTransformComponent : Entity, IAwake<Transform, AttachPointCollector, bool>, IUpdate, ILSRollback
	{
		public Transform Transform { get; set; }
		public AttachPointCollector AttachPointCollector;

		public Quaternion Rotation;
		public Vector3 Position;
		public Vector3 CurrentVelocity;
		public bool IsUesViewRotation;
		public bool Enabled = true;
	}
}