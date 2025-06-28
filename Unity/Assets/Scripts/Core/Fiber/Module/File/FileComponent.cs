using System;
using System.Collections.Generic;

namespace ET
{
    public class FileComponent: Singleton<FileComponent>, ISingletonAwake
    {
        public struct FileLoader
        {
            public string Name { get; set; }
        }

        private readonly Dictionary<string, byte[]> files = new();
        
        public void Awake()
        {
        }
        
        public async ETTask<byte[]> Get(string name)
        {
            lock (this)
            {
                if (this.files.TryGetValue(name, out byte[] bytes))
                {
                    return bytes;
                }
            }
            
            byte[] buffer = await EventSystem.Instance.Invoke<FileLoader, ETTask<byte[]>>(new FileLoader() { Name = name });
            if (buffer.Length == 0)
            {
                throw new Exception($"no file data: {name}");
            }

            lock (this)
            {
                this.files[name] = buffer;
            }

            return buffer;
        }
    }
}