using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf(typeof(Room))]
	public class LSViewTimerComponent : Entity, IAwake, IUpdate, ILSRollback
	{
		public float TimerSpeed;
		public long PrevFrameTime;
		
		public readonly Dictionary<long, EntityRef<LSViewTimer>> Timers = new();
		public readonly Queue<EntityRef<LSViewTimer>> RemoveTimers = new();
		public readonly Queue<EntityRef<LSViewTimer>> TimeoutTimers = new();
	}
	
	[ChildOf(typeof(LSViewTimerComponent))]
	public class LSViewTimer : Entity, IAwake<float, int, object, int>
	{
		public float ElapseTime;
		public float Time;
		public int Loop;
		
		public int Type { get; set; }
		public object Object { get; set; }
	}
}