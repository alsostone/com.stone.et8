namespace ET.Server
{

    [ComponentOf(typeof (Player))]
    public class PlayerRoomComponent: Entity, IAwake
    {
        public ActorId RoomActorId { get; set; }
        public int RoomSeatIndex { get; set; }
    }
}