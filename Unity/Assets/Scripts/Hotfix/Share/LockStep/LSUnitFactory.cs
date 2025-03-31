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
	        lsUnit.AddComponent<PropComponent>().Set(NumericType.Speed, 60000);
			lsUnit.AddComponent<LSInputComponent>();
			lsUnit.AddComponent<SkillComponent, int[]>(new int[1] {10000001});
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
