using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    [MemoryPackable]
    [Message(LockStepOuter.C2G_Match)]
    [ResponseType(nameof(G2C_Match))]
    public partial class C2G_Match : MessageObject, ISessionRequest
    {
        public static C2G_Match Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2G_Match), isFromPool) as C2G_Match;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int StageId { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.StageId = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.G2C_Match)]
    public partial class G2C_Match : MessageObject, ISessionResponse
    {
        public static G2C_Match Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Match), isFromPool) as G2C_Match;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        [MemoryPackOrder(1)]
        public int Error { get; set; }

        [MemoryPackOrder(2)]
        public string Message { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.Error = default;
            this.Message = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 匹配成功，通知客户端进入房间
    /// </summary>
    [MemoryPackable]
    [Message(LockStepOuter.Match2G_MatchSuccess)]
    public partial class Match2G_MatchSuccess : MessageObject, IMessage
    {
        public static Match2G_MatchSuccess Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Match2G_MatchSuccess), isFromPool) as Match2G_MatchSuccess;
        }

        [MemoryPackOrder(0)]
        public int RpcId { get; set; }

        /// <summary>
        /// 房间的ActorId
        /// </summary>
        [MemoryPackOrder(1)]
        public ActorId ActorId { get; set; }

        [MemoryPackOrder(2)]
        public byte SeatIndex { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.RpcId = default;
            this.ActorId = default;
            this.SeatIndex = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 客户端通知要进入房间
    /// </summary>
    [MemoryPackable]
    [Message(LockStepOuter.C2Room_JoinRoom)]
    public partial class C2Room_JoinRoom : MessageObject, IRoomMessage
    {
        public static C2Room_JoinRoom Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_JoinRoom), isFromPool) as C2Room_JoinRoom;
        }

        /// <summary>
        /// / 座位索引 不需要客户端赋值
        /// </summary>
        [MemoryPackOrder(0)]
        public byte SeatIndex { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.SeatIndex = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 房间通知客户端进入战斗
    /// </summary>
    [MemoryPackable]
    [Message(LockStepOuter.Room2C_Enter)]
    public partial class Room2C_Enter : MessageObject, IMessage
    {
        public static Room2C_Enter Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_Enter), isFromPool) as Room2C_Enter;
        }

        [MemoryPackOrder(0)]
        public LockStepMatchInfo MatchInfo { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.MatchInfo = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 客户端通知房间加载进度
    /// </summary>
    [MemoryPackable]
    [Message(LockStepOuter.C2Room_LoadingProgress)]
    public partial class C2Room_LoadingProgress : MessageObject, IRoomMessage
    {
        public static C2Room_LoadingProgress Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_LoadingProgress), isFromPool) as C2Room_LoadingProgress;
        }

        /// <summary>
        /// / 座位索引 不需要客户端赋值
        /// </summary>
        [MemoryPackOrder(0)]
        public byte SeatIndex { get; set; }

        /// <summary>
        /// 场景加载进度 0~100 100表示加载完成
        /// </summary>
        [MemoryPackOrder(1)]
        public int Progress { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.SeatIndex = default;
            this.Progress = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.LockStepUnitInfo)]
    public partial class LockStepUnitInfo : MessageObject
    {
        public static LockStepUnitInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(LockStepUnitInfo), isFromPool) as LockStepUnitInfo;
        }

        [MemoryPackOrder(0)]
        public long PlayerId { get; set; }

        [MemoryPackOrder(1)]
        public TrueSync.TSVector Position { get; set; }

        [MemoryPackOrder(2)]
        public TrueSync.TSQuaternion Rotation { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.PlayerId = default;
            this.Position = default;
            this.Rotation = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.LockStepMatchInfo)]
    public partial class LockStepMatchInfo : MessageObject
    {
        public static LockStepMatchInfo Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(LockStepMatchInfo), isFromPool) as LockStepMatchInfo;
        }

        /// <summary>
        /// / 关卡Id
        /// </summary>
        [MemoryPackOrder(0)]
        public int StageId { get; set; }

        /// <summary>
        /// / 房间的ActorId
        /// </summary>
        [MemoryPackOrder(1)]
        public ActorId ActorId { get; set; }

        [MemoryPackOrder(2)]
        public long MatchTime { get; set; }

        [MemoryPackOrder(3)]
        public int Seed { get; set; }

        [MemoryPackOrder(4)]
        public List<LockStepUnitInfo> UnitInfos { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.StageId = default;
            this.ActorId = default;
            this.MatchTime = default;
            this.Seed = default;
            this.UnitInfos.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    /// <summary>
    /// 房间通知客户端进入战斗
    /// </summary>
    [MemoryPackable]
    [Message(LockStepOuter.Room2C_Start)]
    public partial class Room2C_Start : MessageObject, IMessage
    {
        public static Room2C_Start Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_Start), isFromPool) as Room2C_Start;
        }

        [MemoryPackOrder(0)]
        public long StartTime { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.StartTime = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.C2Room_FrameMessage)]
    public partial class C2Room_FrameMessage : MessageObject, IRoomMessage
    {
        public static C2Room_FrameMessage Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_FrameMessage), isFromPool) as C2Room_FrameMessage;
        }

        /// <summary>
        /// / 座位索引 不需要客户端赋值
        /// </summary>
        [MemoryPackOrder(0)]
        public byte SeatIndex { get; set; }

        /// <summary>
        /// / 帧号 服务器用来判定是否需要丢弃
        /// </summary>
        [MemoryPackOrder(1)]
        public int Frame { get; set; }

        /// <summary>
        /// / 操作指令 内含位处理
        /// </summary>
        [MemoryPackOrder(2)]
        public ulong Command { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.SeatIndex = default;
            this.Frame = default;
            this.Command = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.Room2C_FrameMessage)]
    public partial class Room2C_FrameMessage : MessageObject, IMessage
    {
        public static Room2C_FrameMessage Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_FrameMessage), isFromPool) as Room2C_FrameMessage;
        }

        /// <summary>
        /// / 帧号 空帧不下发时不连续
        /// </summary>
        [MemoryPackOrder(0)]
        public int Frame { get; set; }

        /// <summary>
        /// / 帧序号 必须连续
        /// </summary>
        [MemoryPackOrder(1)]
        public int FrameIndex { get; set; }

        /// <summary>
        /// / 操作指令 内含位处理
        /// </summary>
        [MemoryPackOrder(2)]
        public List<ulong> Commands { get; set; } = new();

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Frame = default;
            this.FrameIndex = default;
            this.Commands.Clear();

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.C2Room_TimeAdjust)]
    public partial class C2Room_TimeAdjust : MessageObject, IRoomMessage
    {
        public static C2Room_TimeAdjust Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_TimeAdjust), isFromPool) as C2Room_TimeAdjust;
        }

        /// <summary>
        /// / 座位索引 不需要客户端赋值
        /// </summary>
        [MemoryPackOrder(0)]
        public byte SeatIndex { get; set; }

        [MemoryPackOrder(1)]
        public int Frame { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.SeatIndex = default;
            this.Frame = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.Room2C_TimeAdjust)]
    public partial class Room2C_TimeAdjust : MessageObject, IMessage
    {
        public static Room2C_TimeAdjust Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_TimeAdjust), isFromPool) as Room2C_TimeAdjust;
        }

        [MemoryPackOrder(0)]
        public int DiffTime { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.DiffTime = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.C2Room_CheckHash)]
    public partial class C2Room_CheckHash : MessageObject, IRoomMessage
    {
        public static C2Room_CheckHash Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(C2Room_CheckHash), isFromPool) as C2Room_CheckHash;
        }

        /// <summary>
        /// / 座位索引 不需要客户端赋值
        /// </summary>
        [MemoryPackOrder(0)]
        public byte SeatIndex { get; set; }

        [MemoryPackOrder(1)]
        public int Frame { get; set; }

        [MemoryPackOrder(2)]
        public long Hash { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.SeatIndex = default;
            this.Frame = default;
            this.Hash = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.Room2C_CheckHashFail)]
    public partial class Room2C_CheckHashFail : MessageObject, IMessage
    {
        public static Room2C_CheckHashFail Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(Room2C_CheckHashFail), isFromPool) as Room2C_CheckHashFail;
        }

        [MemoryPackOrder(0)]
        public int Frame { get; set; }

        [MemoryPackOrder(1)]
        public byte[] LSWorldBytes { get; set; }

        [MemoryPackOrder(2)]
        public byte[] LSProcessBytes { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.Frame = default;
            this.LSWorldBytes = default;
            this.LSProcessBytes = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    [MemoryPackable]
    [Message(LockStepOuter.G2C_Reconnect)]
    public partial class G2C_Reconnect : MessageObject, IMessage
    {
        public static G2C_Reconnect Create(bool isFromPool = false)
        {
            return ObjectPool.Instance.Fetch(typeof(G2C_Reconnect), isFromPool) as G2C_Reconnect;
        }

        [MemoryPackOrder(0)]
        public long StartTime { get; set; }

        [MemoryPackOrder(1)]
        public LockStepMatchInfo MatchInfo { get; set; }

        [MemoryPackOrder(2)]
        public byte[] LSWorldBytes { get; set; }

        public override void Dispose()
        {
            if (!this.IsFromPool)
            {
                return;
            }

            this.StartTime = default;
            this.MatchInfo = default;
            this.LSWorldBytes = default;

            ObjectPool.Instance.Recycle(this);
        }
    }

    public static class LockStepOuter
    {
        public const ushort C2G_Match = 11002;
        public const ushort G2C_Match = 11003;
        public const ushort Match2G_MatchSuccess = 11004;
        public const ushort C2Room_JoinRoom = 11005;
        public const ushort Room2C_Enter = 11006;
        public const ushort C2Room_LoadingProgress = 11007;
        public const ushort LockStepUnitInfo = 11008;
        public const ushort LockStepMatchInfo = 11009;
        public const ushort Room2C_Start = 11010;
        public const ushort C2Room_FrameMessage = 11011;
        public const ushort Room2C_FrameMessage = 11012;
        public const ushort C2Room_TimeAdjust = 11013;
        public const ushort Room2C_TimeAdjust = 11014;
        public const ushort C2Room_CheckHash = 11015;
        public const ushort Room2C_CheckHashFail = 11016;
        public const ushort G2C_Reconnect = 11017;
    }
}