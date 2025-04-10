using TrueSync;

namespace ET
{
    public static partial class EffectExecutor
    {
        private static void AddBuff(int[] param, LSUnit owner, LSUnit target)
        {
            target.GetComponent<BuffComponent>().AddBuffs(param, owner);
        }
        private static void AddProperty(int[] param, LSUnit owner, LSUnit target)
        {
            // if (param.Count != 3) { return; }
            // var propType = (kPropertyType)param[0];
            // var propOpType = (kPropertyOpType)param[1];
            // var value = new FP(param[2]) * FP.EN4;
            //
            // target.Get<Entity>().ComProperty.AddPropertyValue(propType, propOpType, value);
        }
        private static void AddDynamicProperty(int[] param, LSUnit owner, LSUnit target)
        {
            // if (param.Count < 3) { return; }
            // var targetEntity = target.Get<Entity>();
            //
            // var propType = (kPropertyType)param[0];
            // var propOpType = (kPropertyOpType)param[1];
            // var value = new FP(param[2]) * FP.EN4;
            // var createWater = param.Count > 3 && param[3] > 0;
            //
            // if (propOpType == kPropertyOpType.P || propOpType == kPropertyOpType.Bp)
            // {
            //     switch (propType)
            //     {
            //         case kPropertyType.Hp:
            //             value *= targetEntity.ComProperty.GetPropertyValue(kPropertyType.HpMax);
            //             break;
            //         case kPropertyType.Water:
            //             value *= targetEntity.ComProperty.GetPropertyValue(kPropertyType.WaterMax);
            //             break;
            //     }
            // }
            //
            // var change = targetEntity.ComProperty.AddDynamicPropertyValue(propType, value);
            // // 产水
            // if (change && propType == kPropertyType.Water && createWater)
            // {
            //     var position = BattleRandom.Range(targetEntity.ComTransform.Bound, FP.EN1 * 2);
            //     position.y = FP.EN1 * 3;
            //     var waterDrop = BattleWorld.Instance.EntityMgr.CreateDrop(9000, targetEntity.ComTeam.TeamFlag, position, FPVector.zero);
            //     targetEntity.ComContainer.PutContent(waterDrop.Handle, target);
            // }
        }
        private static void SubProperty(int[] param, LSUnit owner, LSUnit target)
        {
            // if (param.Count != 3) { return; }
            // var propType = (kPropertyType)param[0];
            // var propOpType = (kPropertyOpType)param[1];
            // var value = new FP(param[2]) * FP.EN4;
            // target.Get<Entity>().ComProperty.AddPropertyValue(propType, propOpType, -value);
        }
        private static void ChangeState(int[] param, LSUnit owner, LSUnit target)
        {
        }
        private static void AddRestrict(int[] param, LSUnit owner, LSUnit target)
        {
            // foreach (var i in param) {
            //     target.Get<Entity>()?.ComStatus.AddStatus((kStatusType)i);
            // }
        }
        private static void AddBullet(int[] param, LSUnit owner, LSUnit target)
        {
            var position = TSVector.zero;
            if (param.Length >= 4) {
                position = new TSVector(param[1], param[2], param[3]) * FP.EN4;
            }else if (param.Length >= 3) {
                position = new TSVector(param[1], param[2], 0) * FP.EN4;
            }else if (param.Length >= 2) {
                position = new TSVector(param[1], 0, 0) * FP.EN4;
            }
            position = position.Rotation(owner.Rotation.eulerAngles.y);
            
            var rotation = TSQuaternion.LookRotation(target.Position - owner.Position);
            LSUnitFactory.CreateBullet(owner.GetParent<LSWorld>(), param[0], owner.Position + position, rotation, owner, target);
        }
        
        private static void DoDamage(int[] param, LSUnit owner, LSUnit target)
        {
            var damage = owner.GetComponent<PropComponent>().Get(NumericType.Atk);
            if (param.Length > 0) { damage += (damage * param[0]) / LSConstValue.PropPrecision; }
            target.GetComponent<BeHitComponent>()?.BeDamage(owner, damage);
        }
        
        private static void DoResearch(int[] param, LSUnit owner, LSUnit target, EntityRef<Entity> carrier)
        {
            // if (param.Count != 2) { return; }
            //
            // var distances = BattleListPoolMgr.Instance.Get<List<EntityDistance>>();
            // TargetSearcher.Search(param[0], carrier.Get<Entity>(), distances);
            // EffectExecutor.Execute(param[1], owner, distances);
            // foreach (var distance in distances) {
            //     BattleClassPoolMgr.Instance.Return(distance);
            // }
            // BattleListPoolMgr.Instance.Return(distances);
        }
        
        private static void SummonSoldier(int[] param, LSUnit owner, LSUnit target)
        {
            // var entityTarget = target.Get<Entity>();
            // var transformOwner = owner.Get<Entity>().ComTransform;
            // var transformTarget = entityTarget.ComTransform;
            //
            // var rotation = transformTarget.Rotation;
            // var position = FPVector.zero;
            // if (param.Count >= 4) {
            //     position = new FPVector(param[1], param[2], param[3]) * FP.EN4;
            // }else if (param.Count >= 3) {
            //     position = new FPVector(param[1], param[2], 0) * FP.EN4;
            // }else if (param.Count >= 2) {
            //     position = new FPVector(param[1], 0, 0) * FP.EN4;
            // }
            // position = position.Rotation(transformOwner.Angle);
            //
            // BattleWorld.Instance.EntityMgr.CreateSoldier(param[0], entityTarget.ComTeam.TeamFlag, transformOwner.Position + position, rotation);
        }
        
        private static void GenDrop(int[] param, LSUnit owner, LSUnit target)
        {
            // var entityTarget = target.Get<Entity>();
            // var teamFlag = entityTarget.ComTeam.TeamFlag;
            // var plant = BattleWorld.Instance.EntityMgr.CreateDrop(param[0], teamFlag, entityTarget.ComTransform.Position, FPVector.zero);
            // entityTarget.ComContainer.PutContent(plant.Handle, entityTarget.Handle);
        }
        
        private static void AddSeed(int[] param, LSUnit owner, LSUnit target)
        {
            // var entityTarget = target.Get<Entity>();
            // entityTarget.ComSeedTable?.AddSeed(param[BattleWorld.Instance.GlobalGrowthMgr.GetGlobalGrowthLevel() - 1]);
        }
        
        private static void RandomGenDrop(int[] param, LSUnit owner, LSUnit target)
        {
            // if (param.Count < 5 || (param.Count - 2) % 3 != 0) {
            //     return;
            // }
            //
            // int dropId = param[0];
            // int count = param[1];
            // var entityTarget = target.Get<Entity>();
            // var dropBounds = BattleListPoolMgr.Instance.Get<List<FPBounds>>();
            // for (int i = 2; i < param.Count; i+=3) {
            //     var size = new FP(param[i + 2]) * FP.EN4;
            //     dropBounds.Add(new FPBounds(new FPVector(param[i], FP.EN1 * 2, param[i + 1]),
            //         new FPVector(size, FP.Zero, size)));
            // }
            //
            // for (int i = 0; i < count; i++) {
            //     int dropIndex = BattleRandom.Range(0, dropBounds.Count);
            //     var position = BattleRandom.Range(dropBounds[dropIndex], FP.Zero);
            //     var drop = BattleWorld.Instance.EntityMgr.CreateDrop(dropId, TeamFlag.None, position, FPVector.zero, true);
            //     BattleWorld.Instance.EntityMgr.AddToGround(drop);
            // }
            // BattleListPoolMgr.Instance.Return(dropBounds);
        }
    }
}