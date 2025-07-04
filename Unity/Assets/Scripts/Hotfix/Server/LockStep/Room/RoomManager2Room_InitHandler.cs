using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    public class RoomManager2Room_InitHandler: MessageHandler<Scene, RoomManager2Room_Init, Room2RoomManager_Init>
    {
        protected override async ETTask Run(Scene root, RoomManager2Room_Init request, Room2RoomManager_Init response)
        {
            Room room = root.AddComponent<Room>();
            room.Name = request.SceneName;
            room.AddComponent<RoomServerComponent, List<long>>(request.PlayerIds);

            await ETTask.CompletedTask;
        }
    }
}