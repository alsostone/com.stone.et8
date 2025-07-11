using System;
using System.Collections.Generic;

namespace ET.Server
{

    [FriendOf(typeof(MatchComponent))]
    public static partial class MatchComponentSystem
    {
        public static async ETTask Match(this MatchComponent self, long playerId)
        {
            if (self.waitMatchPlayers.Contains(playerId))
            {
                return;
            }
            
            self.waitMatchPlayers.Add(playerId);

            if (self.waitMatchPlayers.Count < LSConstValue.MatchCount)
            {
                return;
            }
            
            // 申请一个房间
            TbStartSceneRow tbStartSceneRow = RandomGenerator.RandomArray(TbStartScene.Instance.Maps);
            Match2Map_GetRoom match2MapGetRoom = Match2Map_GetRoom.Create();
            match2MapGetRoom.SceneName = tbStartSceneRow.Name;
            foreach (long id in self.waitMatchPlayers)
            {
                match2MapGetRoom.PlayerIds.Add(id);
            }
            self.waitMatchPlayers.Clear();

            Scene root = self.Root();
            Map2Match_GetRoom map2MatchGetRoom = await root.GetComponent<MessageSender>().Call(
                tbStartSceneRow.ActorId, match2MapGetRoom) as Map2Match_GetRoom;

            MessageLocationSenderComponent messageLocationSenderComponent = root.GetComponent<MessageLocationSenderComponent>();
            for (byte index = 0; index < match2MapGetRoom.PlayerIds.Count; index++)  // 这里发送消息线程不会修改PlayerInfo，所以可以直接使用
            {
                Match2G_MatchSuccess match2GMatchSuccess = Match2G_MatchSuccess.Create();
                match2GMatchSuccess.ActorId = map2MatchGetRoom.ActorId;
                match2GMatchSuccess.SeatIndex = index;
                
                long id = match2MapGetRoom.PlayerIds[index];
                messageLocationSenderComponent.Get(LocationType.Player).Send(id, match2GMatchSuccess);
                // 等待进入房间的确认消息，如果超时要通知所有玩家退出房间，重新匹配
            }
        }
    }

}