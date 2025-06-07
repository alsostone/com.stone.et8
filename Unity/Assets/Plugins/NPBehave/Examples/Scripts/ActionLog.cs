using MemoryPack;
using UnityEngine;

namespace NPBehave.Examples
{
    [MemoryPackable]
    public partial class ActionLog : Action
    {
        [MemoryPackInclude] private readonly string text;
        
        public ActionLog(string text)
        {
            this.text = text;
        }
        
        protected override bool OnAction()
        {
            Debug.Log(text);
            return true;
        }
    }
}