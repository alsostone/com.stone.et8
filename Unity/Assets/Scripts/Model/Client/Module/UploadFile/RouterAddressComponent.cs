using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class UploadFileAddressComponent: Entity, IAwake<string, int>
    {
        public string UploadFileHost;
        public int UploadFilePort;
    }
}