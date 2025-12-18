using System.Collections.Generic;
using TrueSync;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    [FriendOf(typeof (RoomServerComponent))]
    public class C2Room_JoinRoomHandler: MessageHandler<Scene, C2Room_JoinRoom>
    {
        protected override async ETTask Run(Scene root, C2Room_JoinRoom message)
        {
            using C2Room_JoinRoom _ = message;  // 让消息回到池中
            
            Room room = root.GetComponent<Room>();
            RoomServerComponent roomServerComponent = room.GetComponent<RoomServerComponent>();
            RoomPlayer roomPlayer = roomServerComponent.GetRoomPlayer(message.SeatIndex);
            roomPlayer.IsAccept = true;
            
            if (!roomServerComponent.IsAllPlayerAccept())
            {
                return;
            }
            
            LockStepMatchInfo matchInfo = LockStepMatchInfo.Create();
            matchInfo.StageId = room.StageId;
            matchInfo.ActorId = root.GetActorId();
            matchInfo.MatchTime = TimeInfo.Instance.ServerFrameTime();
            matchInfo.Seed = (int)TimeInfo.Instance.ServerFrameTime();
            foreach (long playerId in roomServerComponent.PlayerIds)
            {
                LockStepUnitInfo lockStepUnitInfo = LockStepUnitInfo.Create();
                lockStepUnitInfo.PlayerId = playerId;
                lockStepUnitInfo.HeroSkinId = 100201; // 操作的英雄ID 如果为0则是没有控制英雄单位
                lockStepUnitInfo.Position = new TSVector(0, 0, -5);
                lockStepUnitInfo.Rotation = TSQuaternion.identity;
                matchInfo.UnitInfos.Add(lockStepUnitInfo);
            }

            room.LockStepMode = LockStepMode.Server;
            room.Replay.MatchInfo = matchInfo;
            
            LSWorld lsWorld = new LSWorld(SceneType.LockStepServer);
            room.InitNewWorld(lsWorld, matchInfo);
            
            Room2C_Enter room2CEnter = Room2C_Enter.Create();
            room2CEnter.MatchInfo = matchInfo;
            RoomMessageHelper.Broadcast(room, room2CEnter);
            await ETTask.CompletedTask;
        }
    }
}