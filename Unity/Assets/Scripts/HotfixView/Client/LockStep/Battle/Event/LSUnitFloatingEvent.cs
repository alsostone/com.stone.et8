using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    [FriendOf(typeof(LSUnitView))]
    public class LSUnitFloatingEvent : AEvent<LSWorld, LSUnitFloating>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitFloating args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var position = view.Position + Vector3.up;
            switch (args.Type)
            {
                case FloatingType.Damage:
                    FloatingTextSpawner.instance.FloatingDamageNumber(args.Value.AsFloat(), position);
                    break;
                case FloatingType.Heal:
                    FloatingTextSpawner.instance.FloatingHealNumber(args.Value.AsFloat(), position);
                    break;
                case FloatingType.Exp:
                    FloatingTextSpawner.instance.FloatingExpNumber(args.Value.AsFloat(), position, view.Transform);
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