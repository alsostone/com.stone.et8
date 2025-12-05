using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    [FriendOf(typeof(LSViewTransformComponent))]
    public class LSUnitFloatingEvent : AEvent<LSWorld, LSUnitFloating>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitFloating args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var transformComponent = view.GetComponent<LSViewTransformComponent>();
            var position = transformComponent.GetAttachPoint(AttachPoint.Head);
            switch (args.Type)
            {
                case FloatingType.Damage:
                    FloatingTextSpawner.instance.FloatingDamageNumber(args.Value.AsFloat(), position);
                    break;
                case FloatingType.Heal:
                    FloatingTextSpawner.instance.FloatingHealNumber(args.Value.AsFloat(), position);
                    break;
                case FloatingType.Exp:
                    FloatingTextSpawner.instance.FloatingExpNumber(args.Value.AsFloat(), position, transformComponent.Transform);
                    break;
                default:
                    var value = (int)args.Value - (int)FloatingType.Dodge;
                    FloatingTextSpawner.instance.FloatingTextNumber(value, position);
                    break;
            }
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitCastingEvent: AEvent<LSWorld, LSUnitCasting>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitCasting args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewSkillComponent = view.GetComponent<LSViewSkillComponent>();
            viewSkillComponent?.OnCastSkill(args.SkillId);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitFxEvent: AEvent<LSWorld, LSUnitFx>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitFx args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewEffectComponent = view.GetComponent<ViewEffectComponent>();
            if (viewEffectComponent == null)
                return;
            await viewEffectComponent.PlayFx(args.FxId);
        }
    }
}