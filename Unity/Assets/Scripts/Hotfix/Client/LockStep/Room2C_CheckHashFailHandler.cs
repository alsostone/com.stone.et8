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
            UploadFileAddressComponent fileAddressComponent = root.GetComponent<UploadFileAddressComponent>();
            Room room = root.GetComponent<Room>();
            long myId = root.GetComponent<PlayerComponent>().MyId;

            Log.Error($"Hash Inconsistent. frame: {message.Frame}");
            
            // 如果服务器返回ProcessLog，则保存双端的日志到文件
            if (message.LSProcessBytes != null)
            {
                string filename = GetProcessLogFileSameName(room.Id, myId, message.Frame);
                
                using var stream = room.ProcessLog.GetLogStream();
                using var ms = new MemoryStream();
                BZip2.Compress(stream, ms, false, 6);
                
                await fileAddressComponent.UploadFile(ms.ToArray(), LSConstValue.ProcessFolderName, filename);
                await fileAddressComponent.UploadFile(message.LSProcessBytes, LSConstValue.ProcessFolderNameSvr, filename);
            }
            
            // 如果服务器返回LSWorld，则保存双端的LSWorld到文件
            if (message.LSWorldBytes != null)
            {
                string filename = GetLSWorldFileSameName(room.Id, myId, message.Frame);
                
                LSWorld clientWorld = room.GetLSWorld(message.Frame);
                using (root.AddChild(clientWorld)) {
                    await fileAddressComponent.UploadFile(clientWorld.ToJson().ToUtf8(), LSConstValue.LSWroldFolderName, filename);
                }
                LSWorld serverWorld = MemoryPackHelper.Deserialize(typeof(LSWorld), message.LSWorldBytes, 0, message.LSWorldBytes.Length) as LSWorld;
                using (root.AddChild(serverWorld)) {
                    await fileAddressComponent.UploadFile(serverWorld.ToJson().ToUtf8(), LSConstValue.LSWroldFolderNameSvr, filename);
                }
            }
            await ETTask.CompletedTask;
        }
        
        private string GetProcessLogFileSameName(long key, long id, int frame)
        {
            var filename = new StringBuilder();
            filename.Append("process_log_");
            filename.Append(key).Append("_");
            filename.Append(id).Append("_");
            filename.Append(frame);
            filename.Append("_").Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
            filename.Append(".bin");
            return filename.ToString();
        }
        
        private string GetLSWorldFileSameName(long key, long id, int frame)
        {
            var filename = new StringBuilder();
            filename.Append("frame_lsworld_");
            filename.Append(key).Append("_");
            filename.Append(id).Append("_");
            filename.Append(frame);
            filename.Append("_").Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
            filename.Append(".json");
            return filename.ToString();
        }
        
        private async ETTask SaveToFile(byte[] bytes, string folderName, string fileName)
        {
            if (bytes != null && bytes.Length > 0) {
                var path = Path.Combine(ConstValue.UploadFolder, folderName);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                await Task.Run(() =>
                {
                    var fullname = Path.Combine(path, fileName);
                    using var fs = new FileStream(fullname, FileMode.Create);
                    fs.Write(bytes);
                });
            }
            await ETTask.CompletedTask;
        }
    }
}