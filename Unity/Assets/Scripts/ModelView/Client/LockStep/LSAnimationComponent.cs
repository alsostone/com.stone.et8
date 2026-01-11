using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	public static class AnimationNames
	{
		public const string Idle = "Idle";
		public const string Walk = "Walk";
		public const string Run = "Run";
		public const string Attack = "Attack";
		public const string Skill = "Skill";
	}
	
	[ComponentOf]
	public class LSAnimationComponent : Entity, IAwake<Animation, float>, IDestroy
	{
		public Animation Animation;
		
		public long AnimationTimer;
		public List<string> mAnimationStack = new ();
		public string AnimationName;
		
		public float MontionSpeed;
		public bool isStop;
		public float stopSpeed;
	}
}