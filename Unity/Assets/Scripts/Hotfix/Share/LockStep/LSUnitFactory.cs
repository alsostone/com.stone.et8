using TrueSync;

namespace ET
{
    public static partial class LSUnitFactory
    {
        public static LSUnit Init(LSWorld lsWorld, LockStepUnitInfo unitInfo)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(unitInfo.PlayerId);
			
	        lsUnit.Position = unitInfo.Position;
	        lsUnit.Rotation = unitInfo.Rotation;

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Hero);
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
			propComponent.Set(NumericType.SpeedBase, 60000);
			propComponent.Set(NumericType.AtkSpeedBase, 5000);
			lsUnit.AddComponent<LSInputComponent>();
			lsUnit.AddComponent<DeathComponent, bool>(false);
			lsUnit.AddComponent<BuffComponent>();
			lsUnit.AddComponent<BeHitComponent>();
			lsUnit.AddComponent<SkillComponent, int[]>(new int[] {10000001});
			
			EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
            return lsUnit;
        }
        
        public static LSUnit CreateBullet(LSWorld lsWorld, int bulletId, TSVector position, TSQuaternion rotation, LSUnit caster, LSUnit target)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
			
	        lsUnit.Position = position;
	        lsUnit.Rotation = rotation;

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Bullet);
	        lsUnit.AddComponent<BulletComponent, int, LSUnit, LSUnit>(bulletId, caster, target);
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
    }
}
