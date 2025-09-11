using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	[EntitySystemOf(typeof(LSCacheSceneNameComponent))]
	[FriendOf(typeof(LSCacheSceneNameComponent))]
	public static partial class LSCacheSceneNameComponentSystem
	{
		[EntitySystem]
		private static void Awake(this LSCacheSceneNameComponent self, string name)
		{
			self.Name = name;
		}
		
	}
}
