using System.Collections.Generic;
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
            for (int i = 0; i < param.Length - 1; i+=2)
            {
                if (param[i] == 0) { continue; }
                NumericType type = (NumericType)param[i];
                FP value = param[i + 1] * FP.EN4;
                target.GetComponent<PropComponent>().Add(type, value);
            }
        }
        private static void SubProperty(int[] param, LSUnit owner, LSUnit target)
        {
            for (int i = 0; i < param.Length - 1; i+=2)
            {
                if (param[i] == 0) { continue; }
                NumericType type = (NumericType)param[i];
                FP value = param[i + 1] * FP.EN4;
                target.GetComponent<PropComponent>().Add(type, -value);
            }
        }
        
        private static void AddRealProperty(int[] param, LSUnit owner, LSUnit target)
        {
            if (param.Length < 2) { return; }
            NumericType type = (NumericType)param[0];
            PropComponent propComponent = target.GetComponent<PropComponent>();
            propComponent.AddRealProp(type, param[1] * FP.EN4);
        }
        
        private static void AddRestrict(int[] param, LSUnit owner, LSUnit target)
        {
            if (param.Length == 0) { return; }
            FlagComponent flagComponent = target.GetComponent<FlagComponent>();
            flagComponent.AddRestrict(param[0]);
        }
        private static void RemoveRestrict(int[] param, LSUnit owner, LSUnit target)
        {
            if (param.Length == 0) { return; }
            FlagComponent flagComponent = target.GetComponent<FlagComponent>();
            flagComponent.RemoveRestrict(param[0]);
        }
        
        private static void AddBulletToTarget(int[] param, LSUnit owner, LSUnit target)
        {
            var ownerTransform = owner.GetComponent<TransformComponent>();
            var position = TSVector.zero;
            if (param.Length >= 4) {
                position = new TSVector(param[1], param[2], param[3]) * FP.EN4;
            }else if (param.Length >= 3) {
                position = new TSVector(param[1], param[2], 0) * FP.EN4;
            }else if (param.Length >= 2) {
                position = new TSVector(param[1], 0, 0) * FP.EN4;
            }
            position = position.Rotation(ownerTransform.Rotation.eulerAngles.y);
            LSUnitFactory.CreateBullet(owner.LSWorld(), param[0], ownerTransform.Position + position, owner, target);
        }
        private static void AddBulletToPostion(int[] param, LSUnit owner, TSVector target)
        {
            var ownerTransform = owner.GetComponent<TransformComponent>();
            var position = TSVector.zero;
            if (param.Length >= 4) {
                position = new TSVector(param[1], param[2], param[3]) * FP.EN4;
            }else if (param.Length >= 3) {
                position = new TSVector(param[1], param[2], 0) * FP.EN4;
            }else if (param.Length >= 2) {
                position = new TSVector(param[1], 0, 0) * FP.EN4;
            }
            position = position.Rotation(ownerTransform.Rotation.eulerAngles.y);
            LSUnitFactory.CreateBulletToPosition(owner.LSWorld(), param[0], ownerTransform.Position + position, owner, target);
        }
        
        private static void DoHealing(int[] param, LSUnit owner, LSUnit target)
        {
            FP attack = owner.GetComponent<PropComponent>().Get(NumericType.Atk);
            if (param.Length > 0)
            {
                attack *= param[0] * FP.EN4;
            }
            attack += param.Length > 1 ? param[1] * FP.EN4 : 0;
            target.GetComponent<BeHitComponent>()?.BeHealing(owner, attack);
        }

        private static void DoDamage(int[] param, LSUnit owner, LSUnit target)
        {
            FP attack = owner.GetComponent<PropComponent>().Get(NumericType.Atk);
            if (param.Length > 0)
            {
                attack *= param[0] * FP.EN4;
            }
            attack += param.Length > 1 ? param[1] * FP.EN4 : 0;
            target.GetComponent<BeHitComponent>()?.BeDamage(owner, attack);
        }
        
        private static void DoResearch(int[] param, LSUnit owner, LSUnit target, LSUnit carrier)
        {
            if (param.Length != 2) { return; }
            
            var distances = ObjectPool.Instance.Fetch<List<SearchUnit>>();
            TargetSearcher.Search(param[0], carrier, distances);
            EffectExecutor.Execute(param[1], owner, distances);
            distances.Clear();
            ObjectPool.Instance.Recycle(distances);
        }
        
        private static void SummonSoldier(int[] param, LSUnit owner, LSUnit target)
        {
            var targetTransform = target.GetComponent<TransformComponent>();
            var position = TSVector.zero;
            if (param.Length >= 4) {
                position = new TSVector(param[1], param[2], param[3]) * FP.EN4;
            }else if (param.Length >= 3) {
                position = new TSVector(param[1], param[2], 0) * FP.EN4;
            }else if (param.Length >= 2) {
                position = new TSVector(param[1], 0, 0) * FP.EN4;
            }
            position = position.Rotation(targetTransform.Rotation.eulerAngles.y);
            TeamType team = target.GetComponent<TeamComponent>().GetTeamType();
            LSUnitFactory.CreateSoldier(target.LSWorld(), param[0], targetTransform.Position + position, targetTransform.Rotation, team);
        }
        
        private static void GenDrop(int[] param, LSUnit owner, LSUnit target)
        {
            // var entityTarget = target.Get<Entity>();
            // var teamFlag = entityTarget.ComTeam.TeamFlag;
            // var plant = BattleWorld.Instance.EntityMgr.CreateDrop(param[0], teamFlag, entityTarget.ComTransform.Position, FPVector.zero);
            // entityTarget.ComContainer.PutContent(plant.Handle, entityTarget.Handle);
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