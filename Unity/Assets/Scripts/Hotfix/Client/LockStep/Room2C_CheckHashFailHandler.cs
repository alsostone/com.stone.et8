using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.BZip2;

namespace ET.Client
{
    [MessageHandler(SceneType.LockStep)]
    public class Room2C_CheckHashFailHandler: MessageHandler<Scene, Room2C_CheckHashFail>
    {
        protected override async ETTask Run(Scene root, Room2C_CheckHashFail message)
        {
            Room room = root.GetComponent<Room>();
            if (room.IsInconsistent)
                return;
            
            room.IsInconsistent = true;
            Log.Error($"Hash Inconsistent. frame: {message.Frame}");
            
            // 如果服务器返回ProcessLog，则保存双端的日志到文件
            if (message.LSProcessBytes != null)
            {
                string filename = GetProcessLogFileSameName(message.Frame);
                
                using var stream = room.ProcessLog.GetLogStream();
                using var ms = new MemoryStream();
                BZip2.Compress(stream, ms, false, 6);
                
                await SaveToFile(ms.ToArray(), LSConstValue.ProcessFolderName, filename);
                await SaveToFile(message.LSProcessBytes, LSConstValue.ProcessFolderNameSvr, filename);
            }
            
            // 如果服务器返回LSWorld，则保存双端的LSWorld到文件
            if (message.LSWorldBytes != null)
            {
                string filename = GetLSWorldFileSameName(message.Frame);
                
                LSWorld clientWorld = room.GetLSWorld(message.Frame);
                using (root.AddChild(clientWorld)) {
                    await SaveToFile(clientWorld.ToJson().ToUtf8(), LSConstValue.LSWroldFolderName, filename);
                }
                LSWorld serverWorld = MemoryPackHelper.Deserialize(typeof(LSWorld), message.LSWorldBytes, 0, message.LSWorldBytes.Length) as LSWorld;
                using (root.AddChild(serverWorld)) {
                    await SaveToFile(serverWorld.ToJson().ToUtf8(), LSConstValue.LSWroldFolderNameSvr, filename);
                }
            }
            await ETTask.CompletedTask;
        }
        
        private string GetProcessLogFileSameName(long key)
        {
            var filename = new StringBuilder();
            filename.Append("process_log_");
            filename.Append(key);
            filename.Append("_").Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
            filename.Append(".bin");
            return filename.ToString();
        }
        
        private string GetLSWorldFileSameName(long key)
        {
            var filename = new StringBuilder();
            filename.Append("frame_lsworld_");
            filename.Append(key);
            filename.Append("_").Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
            filename.Append(".json");
            return filename.ToString();
        }
        
        private async ETTask SaveToFile(byte[] bytes, string folderName, string fileName)
        {
            if (bytes != null && bytes.Length > 0) {
                var path = $"/Users/stone/Library/Application Support/test/ET/{folderName}";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                await Task.Run(() =>
                {
                    var fullname = $"{path}/{fileName}";
                    using var fs = new FileStream(fullname, FileMode.Create);
                    fs.Write(bytes);
                });
            }
            await ETTask.CompletedTask;
        }
    }
}