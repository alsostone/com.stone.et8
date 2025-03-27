namespace ET.Server
{

    public static partial class RoomMessageHelper
    {
        public static void Broadcast(Room room, IMessage message)
        {
            RoomServerComponent roomServerComponent = room.GetComponent<RoomServerComponent>();

            MessageLocationSenderComponent messageLocationSenderComponent = room.Root().GetComponent<MessageLocationSenderComponent>();
            foreach (var kv in roomServerComponent.Children)
            {
                RoomPlayer roomPlayer = kv.Value as RoomPlayer;

                if (!roomPlayer.IsOnline)
                {
                    continue;
                }
                
                messageLocationSenderComponent.Get(LocationType.GateSession).Send(roomPlayer.Id, message);
            }
        }
    }
}