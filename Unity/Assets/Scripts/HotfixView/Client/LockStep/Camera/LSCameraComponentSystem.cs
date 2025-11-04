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
			self.OnDragScrolling();
			self.OnLookAtTarget();
			
			// 摄像机每帧更新位置
			Room room = self.Room();
			if (room.LockStepMode < LockStepMode.Local)
			{
				if (Input.GetKeyDown(KeyCode.Tab))
				{
					self.LookSeatIndex = (self.LookSeatIndex + 1) % room.PlayerIds.Count;
					self.LookUnitView = self.GetBindUnitView(self.LookSeatIndex);
				}
			}
			else if (self.LookUnitView == null)
			{
				int seatIndex = room.GetLookSeatIndex();
				self.LookUnitView = self.GetBindUnitView(seatIndex);
			}
			
			if (!self.IsDragging && self.LookUnitView != null && self.IsFlowTarget)
			{
				self.LookPosition = self.LookUnitView.GetComponent<LSViewTransformComponent>().Transform.position;
			}
			
			self.Transform.position = self.LookPosition + new Vector3(0, 20, -2.5f);
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
		
		private static void OnDragScrolling(this LSCameraComponent self)
		{
			LSUnitView lsPlayer = self.LSUnitView(self.Room().LookPlayerId);
			if (lsPlayer.GetComponent<LSViewSelectionComponent>().HasSelectedUnit())
			{
				// 有选中单位时禁止拖拽摄像机移动
				return;
			}
			
			if (Input.GetMouseButtonDown(1))
			{
				self.DragMousePosition = Input.mousePosition;
				self.IsDragging = true;
				return;
			}
			if (!Input.GetMouseButton(1) || Input.GetKeyDown(KeyCode.Escape))
			{
				self.IsDragging = false;
				return;
			}

			if (self.IsDragging && Input.mousePosition != self.DragMousePosition)
			{
				Ray ray = self.Camera.ScreenPointToRay(Input.mousePosition);
				if (self.DragGroundPlane.Raycast(ray, out float distance))
				{
					Vector3 end = ray.GetPoint(distance);
					ray = self.Camera.ScreenPointToRay(self.DragMousePosition);
					if (self.DragGroundPlane.Raycast(ray, out distance))
					{
						Vector3 start = ray.GetPoint(distance);
						Vector3 direction = start - end;
						self.LookPosition += direction * self.DragSpeedRatio;
						self.DragMousePosition = Input.mousePosition;
					}
				}
			}
		}
		
		private static void OnLookAtTarget(this LSCameraComponent self)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				self.IsFlowTarget = !self.IsFlowTarget;
			}
		}
		
	}
}
