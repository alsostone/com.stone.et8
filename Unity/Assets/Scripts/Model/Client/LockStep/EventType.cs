namespace ET.Client
{
    public struct LSSceneChangeStart
    {
        public string SceneName;
    }
    
    public struct LSSceneInitFinish
    {
        public long OwnerId;
        public long LookId;
    }
}