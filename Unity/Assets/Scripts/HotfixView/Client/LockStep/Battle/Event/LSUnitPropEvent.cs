using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    [FriendOf(typeof(LSUnitView))]
    public class LSUnitPropEvent : AEvent<LSWorld, PropChange>
    {
        protected override async ETTask Run(LSWorld lsWorld, PropChange args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            view.GetComponent<LSViewPropComponent>()?.ResetPropValue(args.NumericType, args.New.AsFloat());
            
            switch (args.NumericType)
            {
                case NumericType.Hp:
                {
                    var hudComponent = view.GetComponent<LSViewHudComponent>();
                    hudComponent?.SetHp(args.New.AsFloat());
                    break;
                }
                case NumericType.MaxHp:
                {
                    var hudComponent = view.GetComponent<LSViewHudComponent>();
                    hudComponent?.SetMaxHp(args.New.AsFloat());
                    break;
                }
            }
            await ETTask.CompletedTask;
        }
    }
}