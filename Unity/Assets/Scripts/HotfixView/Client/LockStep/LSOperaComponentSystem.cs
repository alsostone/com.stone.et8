using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSOperaComponent))]
    [FriendOf(typeof(LSOperaComponent))]
    public static partial class LSOperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSOperaComponent self)
        {
        }
        
        [EntitySystem]
        private static void Update(this LSOperaComponent self)
        {
            Room room = self.GetParent<Room>();

            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            if (axis != self.Axis)
            {
                ulong cmd = LSCommand.GenCommandFloat24x2(0, OperateCommandType.Move, axis.x, axis.y);
                self.SendCommandMeesage(room, cmd);
                self.Axis = axis;
            }
            
            if (Input.GetKeyDown(KeyCode.J))
            {
                ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.Attack);
                self.SendCommandMeesage(room, cmd);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.Skill1);
                self.SendCommandMeesage(room, cmd);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.Skill2);
                self.SendCommandMeesage(room, cmd);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.Jump);
                self.SendCommandMeesage(room, cmd);
            }
        }

        private static void SendCommandMeesage(this LSOperaComponent self, Room room, ulong command)
        {
            C2Room_FrameMessage sendFrameMessage = C2Room_FrameMessage.Create();
            sendFrameMessage.Frame = room.PredictionFrame + 1;
            sendFrameMessage.Command = command;
            room.Root().GetComponent<ClientSenderComponent>().Send(sendFrameMessage);
            
            room.GetComponent<LSCommandsComponent>().AddCommand(room.PredictionFrame + 1, command);
        }

    }
}