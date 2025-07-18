﻿using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	[EntitySystemOf(typeof(LSCameraComponent))]
	[FriendOf(typeof(LSCameraComponent))]
	public static partial class LSCameraComponentSystem
	{
		[EntitySystem]
		private static void Awake(this LSCameraComponent self)
		{
			self.Camera = Camera.main;
			self.Camera.transform.rotation = Quaternion.Euler(new Vector3(80, 0, 0));
		}
		
		[EntitySystem]
		private static void LateUpdate(this LSCameraComponent self)
		{
			// 摄像机每帧更新位置
			Room room = self.GetParent<Room>();
			if (room.IsReplay)
			{
				if (Input.GetKeyDown(KeyCode.Tab))
				{
					List<EntityRef<LSUnitView>> views = room.GetComponent<LSUnitViewComponent>().PlayerViews;
					self.index = (self.index + 1) % views.Count;
					self.LookUnitView = views[self.index];
				}
			}
			else if (self.LookUnitView == null)
			{
				List<EntityRef<LSUnitView>> views = room.GetComponent<LSUnitViewComponent>().PlayerViews;
				byte seatIndex = room.GetSeatIndex(room.GetParent<Scene>().GetComponent<PlayerComponent>().MyId);
				self.LookUnitView = views[seatIndex];
			}
			
			if (self.LookUnitView == null)
			{
				return;
			}

			Vector3 pos = self.LookUnitView.GetComponent<LSViewTransformComponent>().Transform.position;
			self.Transform.position = new Vector3(pos.x, pos.y + 20, pos.z - 2.5f);
		}
		
	}
}
