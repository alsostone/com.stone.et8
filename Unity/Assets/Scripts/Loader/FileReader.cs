using System.IO;
using UnityEngine;

namespace ET
{
    [Invoke]
    public class FileReader: AInvokeHandler<FileComponent.FileLoader, ETTask<byte[]>>
    {
        public override async ETTask<byte[]> Handle(FileComponent.FileLoader args)
        {
            if (Define.IsEditor)
            {
                byte[] bytes = await File.ReadAllBytesAsync(Path.Combine("../Config", args.Name));
                return bytes;
            }
            else
            {
                TextAsset v = await ResourcesComponent.Instance.LoadAssetAsync<TextAsset>($"Assets/Bundles/{args.Name}");
                return v.bytes;
            }
        }
    }
}