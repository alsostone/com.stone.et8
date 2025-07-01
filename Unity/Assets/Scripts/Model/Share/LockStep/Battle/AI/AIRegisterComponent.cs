using System;
using System.Collections.Generic;
using NPBehave;

namespace ET
{
    [Code]
    public class AIRegisterComponent: Singleton<AIRegisterComponent>, ISingletonAwake
    {
        private readonly SortedDictionary<string, Type> sortedSet = new();
        
        public void Awake()
        {
            ushort tagBegin = NodeFormatter.UserFormatterTagBegin;
            
            var types = CodeTypes.Instance.GetTypes(typeof (AINodeAttribute));
            foreach (Type type in types)
            {
                sortedSet.Add(type.FullName, type);
            }
            foreach (var pair in sortedSet)
            {
                NodeFormatter.RegisterFormatter(pair.Value, tagBegin++);
            }
        }

    }
}