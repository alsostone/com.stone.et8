using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSOperaDragComponent))]
    [FriendOf(typeof(LSOperaDragComponent))]
    public static partial class LSOperaDragComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSOperaDragComponent self)
        {
        }
        
        [EntitySystem]
        private static void Update(this LSOperaDragComponent self)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return;
                self.OnTouchBegin(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                self.OnTouchEnd(Input.mousePosition);
            }
#else
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if (self.dragFingerId == -1 && touch.phase == UnityEngine.TouchPhase.Began)
                {
                    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        continue;
                    if (self.OnTouchBegin(touch.position))
                    {
                        self.dragFingerId = touch.fingerId;
                        self.OnTouchMove(touch.position);
                    }
                }
                else if (touch.fingerId == self.dragFingerId && (touch.phase == UnityEngine.TouchPhase.Moved || touch.phase == UnityEngine.TouchPhase.Stationary))
                {
                    self.OnTouchMove(touch.position);
                }
                else if (touch.fingerId == self.dragFingerId && touch.phase == UnityEngine.TouchPhase.Ended)
                {
                    self.OnTouchEnd(touch.position);
                    self.dragFingerId = -1;
                }
            }
#endif
            if (self.dragFingerId == -1)
            {
                self.OnTouchMove(Input.mousePosition);
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                self.CancelPlacementObject();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                self.RotatePlacementObject(1);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                int[] arr = new int[3] { 2001, 2002, 2003 };
                self.SetPlacementObject(EUnitType.Block, arr[Random.Range(0, arr.Length)], 1);
            }
        }
        
        private static bool OnTouchBegin(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (!self.isDraging)
            {
                if (self.RaycastTarget(touchPosition, out GameObject target))
                {
                    Placement buiding = target.GetComponent<Placement>();
                    if (buiding != null) {
                        ulong command = LSCommand.GenCommandLong48(0, OperateCommandType.PlacementDragStart, (ulong)buiding.placementData.id);
                        self.SendCommandMeesage(command);
                        self.isDraging = true;
                        return true;
                    }
                }
            }
            return false;
        }

        private static void OnTouchMove(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (self.isDraging)
            {
                if (self.RaycastTerrain(touchPosition, out Vector3 pos)) {
                    ulong command = LSCommand.GenCommandFloat24x2(0, OperateCommandType.PlacementDrag, pos.x, pos.z);
                    self.SendCommandMeesage(command);
                }
            }
        }

        private static void OnTouchEnd(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (self.isDraging)
            {
                if (self.RaycastTerrain(touchPosition, out Vector3 pos))
                {
                    ulong command = LSCommand.GenCommandFloat24x2(0, OperateCommandType.PlacementDragEnd, pos.x, pos.z);
                    self.SendCommandMeesage(command);
                }
                else
                {
                    ulong command = LSCommand.GenCommandButton(0, CommandButtonType.PlacementCancel);
                    self.SendCommandMeesage(command);
                }
                self.isDraging = false;
            }
        }
        
        public static void SetPlacementObject(this LSOperaDragComponent self, EUnitType type, int tableId, int level)
        {
            ulong param = ((ulong)type << 40) | ((ulong)level << 32) | (uint)tableId;
            ulong command = LSCommand.GenCommandLong48(0, OperateCommandType.PlacementStart, param);
            self.SendCommandMeesage(command);
            self.isDraging = true;
        }

        public static void RotatePlacementObject(this LSOperaDragComponent self, int rotation = 1)
        {
            if (self.isDraging)
            {
                ulong command = LSCommand.GenCommandButton(0, CommandButtonType.PlacementRotate, rotation);
                self.SendCommandMeesage(command);
            }
        }
        
        public static void CancelPlacementObject(this LSOperaDragComponent self)
        {
            if (self.isDraging)
            {
                ulong command = LSCommand.GenCommandButton(0, CommandButtonType.PlacementCancel);
                self.SendCommandMeesage(command);
                self.isDraging = false;
            }
        }

        private static bool RaycastTerrain(this LSOperaDragComponent self, Vector3 position, out Vector3 pos)
        {
            pos = default;
            
            LSCameraComponent cameraComponent = self.GetParent<Room>().GetComponent<LSCameraComponent>();
            Ray ray = cameraComponent.Camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, self.raycastDistance, self.terrainMask)) {
                pos = hit.point;
                return true;
            }

            return false;
        }

        private static bool RaycastTarget(this LSOperaDragComponent self, Vector3 position, out GameObject target)
        {
            target = null;
            
            LSCameraComponent cameraComponent = self.GetParent<Room>().GetComponent<LSCameraComponent>();
            Ray ray = cameraComponent.Camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, self.raycastDistance)) {
                target = hit.collider.gameObject;
                return true;
            }
            return false;
        }

        private static void SendCommandMeesage(this LSOperaDragComponent self, ulong command)
        {
            Room room = self.GetParent<Room>();
            C2Room_FrameMessage sendFrameMessage = C2Room_FrameMessage.Create();
            sendFrameMessage.Frame = room.PredictionFrame + 1;
            sendFrameMessage.Command = command;
            room.Root().GetComponent<ClientSenderComponent>().Send(sendFrameMessage);
            
            room.GetComponent<LSCommandsComponent>().AddCommand(room.PredictionFrame + 1, command);
        }

    }
}