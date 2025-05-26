using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    [FriendOf(typeof(LSViewTransformComponent))]
    public class LSUnitFloatingEvent : AEvent<LSWorld, LSUnitFloating>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitFloating args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            if (view == null) return;   // 创建是异步的 可能未完毕
            
            var transformComponent = view.GetComponent<LSViewTransformComponent>();
            var position = transformComponent.Transform.position + Vector3.up;
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
}