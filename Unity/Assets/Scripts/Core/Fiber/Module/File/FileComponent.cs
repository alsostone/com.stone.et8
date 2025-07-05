using System;
using System.Collections.Generic;

namespace ET
{
    public class FileComponent: Singleton<FileComponent>, ISingletonAwake
    {
        public struct FileLoaderAsync
        {
            public string Name { get; set; }
        }
        
        public struct FileLoader
        {
            public string Name { get; set; }
        }

        private readonly Dictionary<string, byte[]> files = new();
        
        public void Awake()
        {
        }
        
        public async ETTask<byte[]> LoadAsync(string name)
        {
            lock (this)
            {
                if (this.files.TryGetValue(name, out byte[] bytes))
                {
                    return bytes;
                }
            }
            
            byte[] buffer = await EventSystem.Instance.Invoke<FileLoaderAsync, ETTask<byte[]>>(new FileLoaderAsync() { Name = name });
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
        
        public byte[] Load(string name)
        {
            lock (this)
            {
                if (this.files.TryGetValue(name, out byte[] bytes))
                {
                    return bytes;
                }
            }
            
            byte[] buffer = EventSystem.Instance.Invoke<FileLoader, byte[]>(new FileLoader() { Name = name });
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