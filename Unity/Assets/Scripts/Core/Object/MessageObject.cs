using System;
using System.ComponentModel;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [DisableNew]
    public abstract class MessageObject: ProtoObject, IMessage, IDisposable, IPool
    {
        public virtual void Dispose()
        {
        }

        [BsonIgnore]
        [MemoryPackIgnore]
        public bool IsFromPool { get; set; }
    }
}