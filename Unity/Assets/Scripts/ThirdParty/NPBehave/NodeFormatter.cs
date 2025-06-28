using System;
using System.Collections.Generic;
using MemoryPack;

#nullable enable
namespace NPBehave
{
    public sealed class NodeFormatter : MemoryPackFormatter<Node>
    {
        static NodeFormatter() => MemoryPackFormatterProvider.Register(new NodeFormatter());
        
        private static readonly Dictionary<Type, ushort> TypeToTag = new Dictionary<Type, ushort>(64);
        private static readonly Dictionary<ushort, Type> TagToType = new Dictionary<ushort, Type>(64);
        private static ushort sCurrentTag = 0;

        public static void TryAddFormatter(Node value)
        {
            var type = value.GetType();
            if (!TypeToTag.ContainsKey(type))
            {
                TypeToTag.Add(type, ++sCurrentTag);
                TagToType.Add(sCurrentTag, type);
            }
        }

#if DOTNET
        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref Node? value)
#else
        public override void Serialize(ref MemoryPackWriter writer, ref Node? value)
#endif
        {
            if (value == null)
            {
                writer.WriteNullUnionHeader();
                return;
            }

            var type = value.GetType();
            if (TypeToTag.TryGetValue(type, out var tag))
            {
                writer.WriteUnionHeader(tag);
                writer.WriteValue(type, value);
            }
            else
            {
                MemoryPackSerializationException.ThrowNotFoundInUnionType(type, typeof(Node));
            }
        }

#if DOTNET
        public override void Deserialize(ref MemoryPackReader reader, scoped ref Node? value)
#else
        public override void Deserialize(ref MemoryPackReader reader, ref Node? value)
#endif
        {
            if (!reader.TryReadUnionHeader(out var tag))
            {
                value = default;
                return;
            }
        
            if (TagToType.TryGetValue(tag, out var type))
            {
                object? v = value;
                reader.ReadValue(type, ref v);
                value = (Node?)v;
            }
            else
            {
                MemoryPackSerializationException.ThrowInvalidTag(tag, typeof(Node));
            }
        }
    }

}