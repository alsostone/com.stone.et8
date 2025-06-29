namespace ET.Server
{

    [ChildOf(typeof (RoomServerComponent))]
    public class RoomPlayer: Entity, IAwake
    {
        public bool IsAccept { get; set; }
        
        public int Progress { get; set; }

        public bool IsOnline { get; set; } = true;
    }
}