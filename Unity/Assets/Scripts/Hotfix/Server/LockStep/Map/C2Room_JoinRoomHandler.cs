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
            Room room = root.GetComponent<Room>();
            RoomServerComponent roomServerComponent = room.GetComponent<RoomServerComponent>();
            RoomPlayer roomPlayer = roomServerComponent.GetChild<RoomPlayer>(message.PlayerId);
            roomPlayer.IsAccept = true;
            
            if (!roomServerComponent.IsAllPlayerAccept())
            {
                return;
            }
            
            LockStepMatchInfo matchInfo = LockStepMatchInfo.Create();
            matchInfo.SceneName = room.Name;
            matchInfo.ActorId = root.GetActorId();
            matchInfo.MatchTime = TimeInfo.Instance.ServerFrameTime();
            matchInfo.Seed = (int)TimeInfo.Instance.ServerFrameTime();
            foreach (RoomPlayer rp in roomServerComponent.Children.Values)
            {
                LockStepUnitInfo lockStepUnitInfo = LockStepUnitInfo.Create();
                lockStepUnitInfo.PlayerId = rp.Id;
                lockStepUnitInfo.Position = new TSVector(20, 0, -10);
                lockStepUnitInfo.Rotation = TSQuaternion.identity;
                matchInfo.UnitInfos.Add(lockStepUnitInfo);
            }

            Room2C_Enter room2CEnter = Room2C_Enter.Create();
            room2CEnter.MatchInfo = matchInfo;
            RoomMessageHelper.Broadcast(room, room2CEnter);
            
            room.Replay.MatchInfo = matchInfo;
            
            LSWorld lsWorld = new LSWorld(SceneType.LockStepServer);
            await room.Init(lsWorld, matchInfo);
        }
    }
}