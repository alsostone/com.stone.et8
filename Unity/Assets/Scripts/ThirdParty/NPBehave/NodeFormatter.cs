using System;
using System.Collections.Generic;
using MemoryPack;

#nullable enable
namespace NPBehave
{
    public sealed class NodeFormatter : MemoryPackFormatter<Node>
    {
        static NodeFormatter()
        {
            MemoryPackFormatterProvider.Register(new NodeFormatter());
            RegisterFormatter(typeof(Parallel), 1);
            RegisterFormatter(typeof(RandomSelector), 2);
            RegisterFormatter(typeof(RandomSequence), 3);
            RegisterFormatter(typeof(Selector), 4);
            RegisterFormatter(typeof(Sequence), 5);
            RegisterFormatter(typeof(BlackboardBool), 6);
            RegisterFormatter(typeof(BlackboardFloat), 7);
            RegisterFormatter(typeof(BlackboardInt), 8);
            RegisterFormatter(typeof(Cooldown), 9);
            RegisterFormatter(typeof(Failer), 10);
            RegisterFormatter(typeof(Inverter), 11);
            RegisterFormatter(typeof(Observer), 12);
            RegisterFormatter(typeof(Random), 13);
            RegisterFormatter(typeof(Repeater), 14);
            RegisterFormatter(typeof(Succeeder), 15);
            RegisterFormatter(typeof(TimeMax), 16);
            RegisterFormatter(typeof(TimeMin), 17);
            RegisterFormatter(typeof(WaitBlackboardKey), 18);
            RegisterFormatter(typeof(WaitSecond), 19);
            RegisterFormatter(typeof(WaitUntilStopped), 20);
            RegisterFormatter(typeof(Root), 21);
        }
        
        public const ushort UserFormatterTagBegin = 32;
        private static readonly Dictionary<Type, ushort> TypeToTag = new Dictionary<Type, ushort>(64);
        private static readonly Dictionary<ushort, Type> TagToType = new Dictionary<ushort, Type>(64);

        public static void RegisterFormatter(Type type, ushort tag)
        {
            if (TagToType.TryGetValue(tag, out var exsit))
            {
                TypeToTag.Remove(exsit);
                TagToType[tag] = type;
                TypeToTag[type] = tag;
            }
            else
            {
                TypeToTag.Add(type, tag);
                TagToType.Add(tag, type);
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