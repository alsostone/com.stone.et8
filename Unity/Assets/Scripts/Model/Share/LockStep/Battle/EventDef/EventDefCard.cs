using System;
using System.Collections.Generic;

namespace ET
{
    public struct LSCardSelectAdd
    {
        public long Id;
        public List<LSRandomDropItem> Cards;
    }
    
    public struct LSCardSelectDone
    {
        public long Id;
        public int Index;
    }
    
    public struct LSCardBagAdd
    {
        public long Id;
        public LSRandomDropItem Item;
    }
    
    public struct LSCardBagRemove
    {
        public long Id;
        public LSRandomDropItem Item;
    }
    
}

