
using UnityEngine;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewHudComponent : Entity, IAwake<Vector3, Transform, float, float>, IUpdate, IDestroy
	{
		public int HudInstance;
		public Vector3 Offset;
		public Transform FollowTransform;
		public float Hp;
		public float MaxHp;
	}
}