using UnityEngine;

namespace NPBehave
{
    public class UnityContext : MonoBehaviour
    {
        private static UnityContext sInstance = null;

        private static UnityContext Instance
        {
            get {
                if (sInstance == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = "~Context";
                    sInstance = (UnityContext)gameObject.AddComponent(typeof(UnityContext));
                    gameObject.isStatic = true;
#if !UNITY_EDITOR
            gameObject.hideFlags = HideFlags.HideAndDontSave;
#endif
                }
                return sInstance;
            }
        }
        
        public static BehaveWorld GetBehaveWorld()
        {
            return Instance.behaveWorld;
        }
        
        public static Blackboard GetSharedBlackboard(string key)
        {
            return Instance.behaveWorld.GetSharedBlackboard(key);
        }
        
        public static Blackboard CreateBlackboard(Blackboard parent = null)
        {
            return Instance.behaveWorld.CreateBlackboard(parent);
        }

        private readonly BehaveWorld behaveWorld = new BehaveWorld();

        void Update()
        {
            behaveWorld.Update(Time.deltaTime);
        }
    }
}