using System;
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
            LSCommandsComponent lsCommandsComponent = room.GetComponent<LSCommandsComponent>();
            
            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            if (axis != self.Axis)
            {
                self.Axis = axis;
                ulong cmd = LSUtils.GenCommandFloat24x2(0, OperateCommandType.Move, axis.x, axis.y);
                lsCommandsComponent.AddCommand(cmd);
                self.SendCommandMeesage(room, cmd);
            }
            
            if (Input.GetKeyDown(KeyCode.J))
            {
                ulong cmd = LSUtils.GenCommandButton(0, CommandButtonType.Attack);
                lsCommandsComponent.AddCommand(cmd);
                self.SendCommandMeesage(room, cmd);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                ulong cmd = LSUtils.GenCommandButton(0, CommandButtonType.Skill1);
                lsCommandsComponent.AddCommand(cmd);
                self.SendCommandMeesage(room, cmd);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                ulong cmd = LSUtils.GenCommandButton(0, CommandButtonType.Skill2);
                lsCommandsComponent.AddCommand(cmd);
                self.SendCommandMeesage(room, cmd);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ulong cmd = LSUtils.GenCommandButton(0, CommandButtonType.Jump);
                lsCommandsComponent.AddCommand(cmd);
                self.SendCommandMeesage(room, cmd);
            }
        }

        private static void SendCommandMeesage(this LSOperaComponent self, Room room, ulong command)
        {
            C2Room_FrameMessage sendFrameMessage = C2Room_FrameMessage.Create();
            sendFrameMessage.Frame = room.PredictionFrame + 1;
            sendFrameMessage.Command = command;
            room.Root().GetComponent<ClientSenderComponent>().Send(sendFrameMessage);
        }

    }
}