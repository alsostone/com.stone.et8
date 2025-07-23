namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class UploadFileAddressComponent: Entity, IAwake<string, int>
    {
        public string UploadFileHost;
        public int UploadFilePort;
    }
}