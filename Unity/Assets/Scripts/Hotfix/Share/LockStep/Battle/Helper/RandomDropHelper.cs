using System;
using System.Collections.Generic;

namespace ET
{
    public static class RandomDropHelper
    {
        public static void Random(this LSWorld lsWorld, int randomBagId, ref List<Tuple<EUnitType, int, int>> randomResults)
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
                    lsWorld.RandomSet(items[0].Id, items[0].Count, ref randomResults);
                    return;
                }
                
                int totalWeight = 0;
                foreach (ItemRandomBag itemRandomBag in items) {
                    totalWeight += itemRandomBag.Weight;
                }

                int randWeight = lsWorld.Random.Range(1, totalWeight + 1);
                foreach (ItemRandomBag itemRandomBag in items) {
                    randWeight -= itemRandomBag.Weight;
                    if (randWeight <= 0) {
                        lsWorld.RandomSet(itemRandomBag.Id, itemRandomBag.Count, ref randomResults);
                        return;
                    }
                }
            }
            else
            {
                foreach (ItemRandomBag itemRandomBag in items)
                {
                    if (lsWorld.Random.Range(1, LSConstValue.Probability + 1) <= itemRandomBag.Weight) {
                        lsWorld.RandomSet(itemRandomBag.Id, itemRandomBag.Count, ref randomResults);
                    }
                }
            }
        }

        private static void RandomSet(this LSWorld lsWorld, int randomSetId, int randomCount, ref List<Tuple<EUnitType, int, int>> randomResults)
        {
            var items = TbRandomSet.Instance.Get(randomSetId).Items;
            if (items.Length <= randomCount) {
                if (items.Length < randomCount) {
                    Log.Error($"RandomSet[{randomSetId}] items count[{items.Length}], but require[{randomCount}]");
                }
                foreach (ItemRandomSet itemRandomSet in items) {
                    int count = itemRandomSet.CountMax > itemRandomSet.CountMin
                        ? lsWorld.Random.Range(itemRandomSet.CountMin, itemRandomSet.CountMax + 1)
                        : itemRandomSet.CountMin;
                    randomResults.Add(new Tuple<EUnitType, int, int>(itemRandomSet.Type, itemRandomSet.Id, count));
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

                int randWeight = lsWorld.Random.Range(1, totalWeight + 1);
                for (int j = 0; j < items.Length; j++)
                {
                    if (alreadyRand.Contains(j))
                        continue;
                    randWeight -= items[j].Weight;
                    if (randWeight <= 0)
                    {
                        int count = items[j].CountMax > items[j].CountMin
                            ? lsWorld.Random.Range(items[j].CountMin, items[j].CountMax + 1)
                            : items[j].CountMin;
                        randomResults.Add(new Tuple<EUnitType, int, int>(items[j].Type, items[j].Id, count));
                        alreadyRand.Add(j);
                        break;
                    }
                }
            }
            ObjectPool.Instance.Recycle(alreadyRand);
        }
    }
    
}