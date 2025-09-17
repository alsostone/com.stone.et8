using System;
using System.Collections.Generic;
using TrueSync;

namespace ET
{
    public static class RandomDropHelper
    {
        public static void Random(TSRandom random, int randomBagId, List<LSRandomDropItem> randomResults)
        {
            if (randomResults == null)
                return;
            randomResults.Clear();

            TbRandomBagRow resRandomBag = TbRandomBag.Instance.Get(randomBagId);
            if (resRandomBag == null || resRandomBag.Items.Length == 0)
                return;

            var items = resRandomBag.Items;
            if (resRandomBag.IsRandomOne)
            {
                if (items.Length == 1) {
                    RandomSet(random, items[0].Id, items[0].Count, randomResults);
                    return;
                }
                
                int totalWeight = 0;
                foreach (ItemRandomBag itemRandomBag in items) {
                    totalWeight += itemRandomBag.Weight;
                }

                int randWeight = random.Range(1, totalWeight + 1);
                foreach (ItemRandomBag itemRandomBag in items) {
                    randWeight -= itemRandomBag.Weight;
                    if (randWeight <= 0) {
                        RandomSet(random, itemRandomBag.Id, itemRandomBag.Count, randomResults);
                        return;
                    }
                }
            }
            else
            {
                foreach (ItemRandomBag itemRandomBag in items)
                {
                    if (random.Range(1, LSConstValue.Probability + 1) <= itemRandomBag.Weight) {
                        RandomSet(random, itemRandomBag.Id, itemRandomBag.Count, randomResults);
                    }
                }
            }
        }

        /// <summary>
        /// 随机获取N类物品（一个ItemRandomSet为一类）
        /// </summary>
        /// <param name="random"></param>
        /// <param name="randomSetId"></param>
        /// <param name="randomCount">要获取几类物品，注意：不是几个哈</param>
        /// <param name="randomResults"></param>
        public static void RandomSet(TSRandom random, int randomSetId, int randomCount, List<LSRandomDropItem> randomResults)
        {
            var items = TbRandomSet.Instance.Get(randomSetId).Items;
            if (items.Length <= randomCount) {
                if (items.Length < randomCount) {
                    Log.Error($"RandomSet[{randomSetId}] items count[{items.Length}], but require[{randomCount}]");
                }
                foreach (ItemRandomSet itemRandomSet in items) {
                    int count = itemRandomSet.CountMax > itemRandomSet.CountMin
                        ? random.Range(itemRandomSet.CountMin, itemRandomSet.CountMax + 1)
                        : itemRandomSet.CountMin;
                    randomResults.Add(new LSRandomDropItem(itemRandomSet.Type, itemRandomSet.Id, count));
                }

                return;
            }

            // 已随机出的索引
            var alreadyRand = ObjectPool.Instance.Fetch<List<int>>();
            for (int i = 0; i < randomCount; i++)
            {
                int totalWeight = 0;
                for (int j = 0; j < items.Length; j++)
                {
                    if (alreadyRand.Contains(j))
                        continue;
                    totalWeight += items[j].Weight;
                }

                int randWeight = random.Range(1, totalWeight + 1);
                for (int j = 0; j < items.Length; j++)
                {
                    if (alreadyRand.Contains(j))
                        continue;
                    randWeight -= items[j].Weight;
                    if (randWeight <= 0)
                    {
                        int count = items[j].CountMax > items[j].CountMin
                            ? random.Range(items[j].CountMin, items[j].CountMax + 1)
                            : items[j].CountMin;
                        randomResults.Add(new LSRandomDropItem(items[j].Type, items[j].Id, count));
                        alreadyRand.Add(j);
                        break;
                    }
                }
            }
            alreadyRand.Clear();
            ObjectPool.Instance.Recycle(alreadyRand);
        }
    }
    
}