using System;
using System.Collections.Generic;

namespace ET
{
    public class LSUpdater: Object
    {
        private readonly SortedDictionary<long, EntityRef<LSEntity>> updateEntities = new();
        private readonly Dictionary<long, EntityRef<LSEntity>> updateEntitiesNew = new();
        private readonly List<long> removeIds = new();
        
        public void Update()
        {
            foreach (var kv in updateEntitiesNew) {
                this.updateEntities.Add(kv.Key, kv.Value);
            }
            updateEntitiesNew.Clear();
            
            foreach (var kv in this.updateEntities)
            {
                LSEntity entity = kv.Value;
                if (entity == null) {
                    this.removeIds.Add(kv.Key);
                    continue;
                }
                LSEntitySystemSingleton.Instance.LSUpdate(entity);
            }

            foreach (long id in this.removeIds) {
                this.updateEntities.Remove(id);
            }
            removeIds.Clear();
        }
        
        public void Add(LSEntity entity)
        {
            this.updateEntitiesNew.Add(entity.Id, entity);
        }
    }
}