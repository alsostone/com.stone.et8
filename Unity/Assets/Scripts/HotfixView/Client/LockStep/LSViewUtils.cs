namespace ET.Client
{
    public static class LSViewUtils
    {
        public static LSUnitView LSViewOwner(this Entity entity)
        {
            if (entity.Parent is LSUnitView lsUnit)
                return lsUnit;
            return entity.Parent.Parent as LSUnitView;
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

    }
}
