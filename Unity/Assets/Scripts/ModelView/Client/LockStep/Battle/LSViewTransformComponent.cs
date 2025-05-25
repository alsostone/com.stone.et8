
using UnityEngine;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewTransformComponent : Entity, IAwake<Transform>, IUpdate, ILSRollback
	{
		public Transform Transform { get; set; }
		public Vector3 Position;
		public Vector3 CurrentVelocity;
		public Quaternion Rotation;
	}
}