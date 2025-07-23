using System.Collections.Generic;
using YIUIFramework;

namespace ET.Client
{
    [GM(EGMType.Common, 1, "上传文件", "上传客户端文件到Http服务器")]
    public class GM_UploadFile: IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new();
        }

        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            UploadFileAddressComponent addressComponent = clientScene.GetComponent<UploadFileAddressComponent>();
            await addressComponent.UploadFile("1234567890".ToByteArray(), "test", "upload.txt");
            return true;
        }
    }
}