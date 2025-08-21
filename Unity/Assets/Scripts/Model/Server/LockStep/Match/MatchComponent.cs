using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class MatchComponent: Entity, IAwake
    {
        public Dictionary<int, List<long>> waitStageMatchPlayers = new ();
        public HashSet<long> waitMatchPlayers = new ();
    }

}