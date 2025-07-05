using System.IO;

namespace ET
{
    [Invoke]
    public class FileReaderAsync: AInvokeHandler<FileComponent.FileLoaderAsync, ETTask<byte[]>>
    {
        public override async ETTask<byte[]> Handle(FileComponent.FileLoaderAsync args)
        {
            byte[] bytes = await File.ReadAllBytesAsync(Path.Combine("../Config", args.Name));
            return bytes;
        }
    }
    
    [Invoke]
    public class FileReader: AInvokeHandler<FileComponent.FileLoader, byte[]>
    {
        public override byte[] Handle(FileComponent.FileLoader args)
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine("../Config", args.Name));
            return bytes;
        }
    }
}