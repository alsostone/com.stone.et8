using System.IO;
using UnityEngine;
using YooAsset;

namespace ET
{
    [Invoke]
    public class FileReaderAsync: AInvokeHandler<FileComponent.FileLoaderAsync, ETTask<byte[]>>
    {
        public override async ETTask<byte[]> Handle(FileComponent.FileLoaderAsync args)
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
    
    [Invoke]
    public class FileReader: AInvokeHandler<FileComponent.FileLoader, byte[]>
    {
        public override byte[] Handle(FileComponent.FileLoader args)
        {
            if (Define.IsEditor)
            {
                byte[] bytes = File.ReadAllBytes(Path.Combine("../Config", args.Name));
                return bytes;
            }
            else
            {
                AssetHandle handle = YooAssets.LoadAssetSync<TextAsset>($"Assets/Bundles/{args.Name}");
                TextAsset t = (TextAsset)handle.AssetObject;
                handle.Release();
                return t.bytes;
            }
        }
    }
}