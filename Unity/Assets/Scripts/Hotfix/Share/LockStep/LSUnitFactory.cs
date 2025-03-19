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

			lsUnit.AddComponent<LSInputComponent>();
            return lsUnit;
        }
        
        public static LSUnit CreateBullet(LSWorld lsWorld, int bulletId, TSVector position, TSQuaternion rotation, LSUnit caster, LSUnit target)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
			
	        lsUnit.Position = position;
	        lsUnit.Rotation = rotation;

	        lsUnit.AddComponent<BulletComponent, int, LSUnit, LSUnit>(bulletId, caster, target);
	        return lsUnit;
        }
    }
}
