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
			self.OnZoomScrolling();
			self.OnSwitchFollowTarget();
			
			// 摄像机每帧更新位置
			Room room = self.Room();
			if (room.LockStepMode < LockStepMode.Local)
			{
				if (Input.GetKeyDown(KeyCode.Tab))
				{
					LSLookComponent lookComponent = room.GetComponent<LSLookComponent>();
					lookComponent.SetLookSeatIndex(lookComponent.GetLookSeatIndex() + 1);
					self.LookUnitView = room.GetLookHeroView();
				}
			}
			else if (self.LookUnitView == null)
			{
				self.LookUnitView = room.GetLookHeroView();
			}
			
			if (!self.IsDragging && self.LookUnitView != null && self.IsFollowTarget)
			{
				self.LookPosition = self.LookUnitView.GetComponent<LSViewTransformComponent>().Transform.position;
			}
			
			self.Transform.position = self.LookPosition - self.Transform.forward * self.FlowDistance;
		}
		
		private static void OnDragScrolling(this LSCameraComponent self)
		{
			LSUnitView lsPlayer = self.Room().GetLookPlayerView();
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
		
		private static void OnZoomScrolling(this LSCameraComponent self)
		{
			float scroll = Input.GetAxis("Mouse ScrollWheel");
			if (scroll != 0)
			{
				self.FlowDistance -= scroll * self.ZoomSpeed;
				self.FlowDistance = Mathf.Clamp(self.FlowDistance, 5, 30);
			}
		}

		private static void OnSwitchFollowTarget(this LSCameraComponent self)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				self.IsFollowTarget = !self.IsFollowTarget;
			}
		}
		
		public static Vector2 ConvertRelativeDirection(this LSCameraComponent self, Vector2 direction)
		{
			if (self.Transform == null)
				return Vector3.zero;
            
			Vector2 forward = self.Transform.forward.GetXZ().normalized;
			Vector2 right = self.Transform.right.GetXZ().normalized;

			direction = forward * direction.y + right * direction.x;
			return direction.normalized;
		}
	}
}
