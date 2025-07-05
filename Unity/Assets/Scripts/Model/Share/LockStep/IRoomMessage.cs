namespace ET
{
    public interface IRoomMessage: IMessage
    {
        byte SeatIndex { get; set; }
    }
}