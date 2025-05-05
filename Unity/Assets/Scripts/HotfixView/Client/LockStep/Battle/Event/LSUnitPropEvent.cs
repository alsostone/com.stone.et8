using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    [FriendOf(typeof(LSUnitView))]
    public class LSUnitPropEvent : AEvent<LSWorld, PropChange>
    {
        protected override async ETTask Run(LSWorld lsWorld, PropChange args)
        {
            if (args.NumericType != NumericType.Hp && args.NumericType != NumericType.MaxHp)
                return;
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var hudComponent = view.GetComponent<LSHudComponent>();
            if (hudComponent == null)
                return;
            switch (args.NumericType)
            {
                case NumericType.Hp:
                    hudComponent.SetHp(args.New * LSConstValue.PropPrecision2);
                    break;
                case NumericType.MaxHp:
                    hudComponent.SetMaxHp(args.New * LSConstValue.PropPrecision2);
                    break;
            }
            await ETTask.CompletedTask;
        }
    }
}