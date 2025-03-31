using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSOperaComponent))]
    [FriendOf(typeof(LSClientUpdater))]
    public static partial class LSOperaComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSOperaComponent self)
        {
        }
        
        [EntitySystem]
        private static void Update(this LSOperaComponent self)
        {
            LSClientUpdater lsClientUpdater = self.GetParent<Room>().GetComponent<LSClientUpdater>();
            TSVector2 v = new TSVector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            lsClientUpdater.Input.V = v.normalized;
            
            if (Input.GetKeyDown(KeyCode.J))
            {
                lsClientUpdater.Input.Button |= LSConstButtonValue.Attack;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                lsClientUpdater.Input.Button |= LSConstButtonValue.Skill1;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                lsClientUpdater.Input.Button |= LSConstButtonValue.Jump;
            }
        }

    }
}