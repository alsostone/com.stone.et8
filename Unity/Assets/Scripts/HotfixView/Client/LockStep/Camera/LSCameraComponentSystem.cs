using System.Collections.Generic;
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
			Room room = self.Room();
			if (room.LockStepMode < LockStepMode.Local)
			{
				if (Input.GetKeyDown(KeyCode.Tab))
				{
					self.index = (self.index + 1) % room.PlayerIds.Count;
					self.LookUnitView = self.GetBindUnitView(self.index);
				}
			}
			else if (self.LookUnitView == null)
			{
				int seatIndex = room.GetLookSeatIndex();
				self.LookUnitView = self.GetBindUnitView(seatIndex);
			}
			
			if (self.LookUnitView == null)
			{
				return;
			}

			Vector3 pos = self.LookUnitView.GetComponent<LSViewTransformComponent>().Transform.position;
			self.Transform.position = new Vector3(pos.x, pos.y + 20, pos.z - 2.5f);
		}
		
		private static LSUnitView GetBindUnitView(this LSCameraComponent self, int seatIndex)
		{
			Room room = self.Room();
			LSUnitViewComponent lsUnitViewComponent = room.GetComponent<LSUnitViewComponent>();
			LSUnitView lsPlayer = lsUnitViewComponent.GetChild<LSUnitView>(room.PlayerIds[seatIndex]);
			
			LSViewPlayerComponent lsViewPlayerComponent = lsPlayer.GetComponent<LSViewPlayerComponent>();
			if (lsViewPlayerComponent.BindViewId == 0)
				return null;
			
			return lsUnitViewComponent.GetChild<LSUnitView>(lsViewPlayerComponent.BindViewId);
		}
	}
}
