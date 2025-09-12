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
            var room = lsWorld.Room();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var hudComponent = view.GetComponent<LSViewHudComponent>();
            if (hudComponent == null)
                return;
            switch (args.NumericType)
            {
                case NumericType.Hp:
                    hudComponent.SetHp(args.New.AsFloat());
                    break;
                case NumericType.MaxHp:
                    hudComponent.SetMaxHp(args.New.AsFloat());
                    break;
            }
            await ETTask.CompletedTask;
        }
    }
}