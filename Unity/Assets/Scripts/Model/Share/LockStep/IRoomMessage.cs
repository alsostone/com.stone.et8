namespace ET
{
    public interface IRoomMessage: IMessage
    {
        int SeatIndex { get; set; }
    }
}