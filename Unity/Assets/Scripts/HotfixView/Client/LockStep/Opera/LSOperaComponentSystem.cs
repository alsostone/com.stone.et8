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
                room.SendCommandMeesage(cmd);
                self.Axis = axis;
            }
            
            if (Input.GetKeyDown(KeyCode.J))
            {
                ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.Attack);
                room.SendCommandMeesage(cmd);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.Skill1);
                room.SendCommandMeesage(cmd);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.Skill2);
                room.SendCommandMeesage(cmd);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.Jump);
                room.SendCommandMeesage(cmd);
            }
        }

    }
}