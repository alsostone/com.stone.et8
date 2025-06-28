using System.IO;

namespace ET
{
    [Invoke]
    public class FileReader: AInvokeHandler<FileComponent.FileLoader, ETTask<byte[]>>
    {
        public override async ETTask<byte[]> Handle(FileComponent.FileLoader args)
        {
            byte[] bytes = await File.ReadAllBytesAsync(Path.Combine("../Config", args.Name));
            return bytes;
        }
    }
}