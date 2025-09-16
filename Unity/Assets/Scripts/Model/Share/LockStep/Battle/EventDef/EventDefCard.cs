using System;
using System.Collections.Generic;

namespace ET
{
    public struct LSCardSelectAdd
    {
        public long Id;
        public List<Tuple<EUnitType, int, int>> Cards;
    }
    
    public struct LSCardSelectDone
    {
        public long Id;
        public int Index;
    }
    
    public struct LSCardBagAdd
    {
        public long Id;
        public EUnitType Type;
        public int TableId;
        public int Count;
    }
    
    public struct LSCardBagRemove
    {
        public long Id;
        public EUnitType Type;
        public int TableId;
        public int Count;
    }
    
}

