namespace ET.Server
{

    public static partial class RoomMessageHelper
    {
        public static void Broadcast(Room room, IMessage message)
        {
            RoomServerComponent roomServerComponent = room.GetComponent<RoomServerComponent>();

            MessageLocationSenderComponent messageLocationSenderComponent = room.Root().GetComponent<MessageLocationSenderComponent>();
            foreach (RoomPlayer roomPlayer in roomServerComponent.Children.Values)
            {
                if (!roomPlayer.IsOnline)
                    continue;
                messageLocationSenderComponent.Get(LocationType.GateSession).Send(roomPlayer.Id, message);
            }
        }
    }
}