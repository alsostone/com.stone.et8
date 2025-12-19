using UnityEngine;

namespace ET.Client
{
    public static class LSViewUtils
    {
        public static Vector2 GetXZ(this Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }
        
        public static LSUnitView LSViewOwner(this Entity entity)
        {
            if (entity.Parent is LSUnitView lsUnit)
                return lsUnit;
            return entity.Parent.Parent as LSUnitView;
        }
        
        public static LSUnitView LSUnitView(this Entity entity, long id)
        {
            LSUnitViewComponent viewComponent = entity.Room().GetComponent<LSUnitViewComponent>();
            return viewComponent?.GetChild<LSUnitView>(id);
        }

        public static void SendCommandMeesage(this Room self, LSCommandData command)
        {
            C2Room_FrameMessage sendFrameMessage = C2Room_FrameMessage.Create();
            sendFrameMessage.Frame = self.PredictionFrame + 1;
            sendFrameMessage.Command = command;
            self.GetComponent<LSCommandsComponent>().AddCommand(self.PredictionFrame + 1, command);
            
            // 联网模式下，客户端发送命令给服务器
            if (self.LockStepMode == LockStepMode.Server) {
                self.Root().GetComponent<ClientSenderComponent>().Send(sendFrameMessage);
            }
        }

        public static int GetLookSeatIndex(this Entity self)
        {
            LSLookComponent lookComponent = self.Room().GetComponent<LSLookComponent>();
            return lookComponent.GetLookSeatIndex();
        }
        
        public static long GetLookPlayerId(this Entity self)
        {
            LSLookComponent lookComponent = self.Room().GetComponent<LSLookComponent>();
            return lookComponent.LookPlayerId;
        }
        
        public static T GetLookPlayerComponent<T>(this Room self) where T : Entity
        {
            LSLookComponent lookComponent = self.GetComponent<LSLookComponent>();
            LSUnitViewComponent lsUnitViewComponent = self.GetComponent<LSUnitViewComponent>();
            LSUnitView lsPlayer = lsUnitViewComponent.GetChild<LSUnitView>(lookComponent.LookPlayerId);
            return lsPlayer.GetComponent<T>();
        }
        
        public static LSUnitView GetLookPlayerView(this Room self)
        {
            LSLookComponent lookComponent = self.GetComponent<LSLookComponent>();
            LSUnitViewComponent lsUnitViewComponent = self.GetComponent<LSUnitViewComponent>();
            return lsUnitViewComponent.GetChild<LSUnitView>(lookComponent.LookPlayerId);
        }

        public static LSUnitView GetLookHeroView(this Room self)
        {
            LSUnitViewComponent lsUnitViewComponent = self.GetComponent<LSUnitViewComponent>();
            if (lsUnitViewComponent == null)	// 被初始化调用时 还没有LSUnitViewComponent
                return null;
			
            LSLookComponent lookComponent = self.GetComponent<LSLookComponent>();
            LSUnitView lsPlayer = lsUnitViewComponent.GetChild<LSUnitView>(lookComponent.LookPlayerId);
            LSViewPlayerComponent lsViewPlayerComponent = lsPlayer.GetComponent<LSViewPlayerComponent>();
            if (lsViewPlayerComponent.BindHeroId == 0)
                return null;
			
            return lsUnitViewComponent.GetChild<LSUnitView>(lsViewPlayerComponent.BindHeroId);
        }
 
    }
}
