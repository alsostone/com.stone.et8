using System;
using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(RoomServerComponent))]
    [FriendOf(typeof(RoomServerComponent))]
    public static partial class RoomServerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RoomServerComponent self, List<long> playerIds)
        {
            self.PlayerIds.AddRange(playerIds);
            for (int index = 0; index < self.PlayerIds.Count; index++)
            {
                long id = self.PlayerIds[index];
                RoomPlayer roomPlayer = self.AddChildWithId<RoomPlayer>(id);
                roomPlayer.AddComponent<LSCommandsComponent, byte>((byte)index);
            }
        }
        
        public static RoomPlayer GetRoomPlayer(this RoomServerComponent self, byte seatIndex)
        {
            long id = self.PlayerIds[seatIndex];
            return self.GetChild<RoomPlayer>(id);
        }

        public static bool IsAllPlayerProgress100(this RoomServerComponent self)
        {
            foreach (RoomPlayer roomPlayer in self.Children.Values)
            {
                if (roomPlayer.Progress < 100)
                {
                    return false;
                }
            }
            return true;
        }
        
        public static bool IsAllPlayerAccept(this RoomServerComponent self)
        {
            foreach (RoomPlayer roomPlayer in self.Children.Values)
            {
                if (!roomPlayer.IsAccept)
                {
                    return false;
                }
            }
            return true;
        }
    }
}