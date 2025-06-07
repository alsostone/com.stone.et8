using UnityEngine;
using System.Collections.Generic;

namespace NPBehave
{
    public class Debugger : MonoBehaviour
    {
        public Root BehaviorTree;

        private static Blackboard _customGlobalStats = null;
        public static Blackboard CustomGlobalStats
        {
            get 
            {
                if (_customGlobalStats == null)
                {
                    _customGlobalStats = UnityContext.GetSharedBlackboard("_GlobalStats");;
                }
                return _customGlobalStats;
            }
        }

        private Blackboard _customStats = null;
        public Blackboard CustomStats
        {
            get 
            {
                if (_customStats == null)
                {
                    _customStats = UnityContext.CreateBlackboard();;
                }
                return _customStats;
            }
        }

        public void DebugCounterInc(string key)
        {
            if (!CustomStats.IsSetInt(key))
            {
                CustomStats.SetInt(key, 0);
            }
            CustomStats.SetInt(key, CustomStats.GetInt(key) + 1);
        }

        public void DebugCounterDec(string key)
        {
            if (!CustomStats.IsSetInt(key))
            {
                CustomStats.SetInt(key, 0);
            }
            CustomStats.SetInt(key, CustomStats.GetInt(key) - 1);
        }

        public static void GlobalDebugCounterInc(string key)
        {
            if (!CustomGlobalStats.IsSetInt(key))
            {
                CustomGlobalStats.SetInt(key, 0);
            }
            CustomGlobalStats.SetInt(key, CustomGlobalStats.GetInt(key) + 1);
        }

        public static void GlobalDebugCounterDec(string key)
        {
            if (!CustomGlobalStats.IsSetInt(key))
            {
                CustomGlobalStats.SetInt(key, 0);
            }
            CustomGlobalStats.SetInt(key, CustomGlobalStats.GetInt(key) + 1);
        }

    }
}