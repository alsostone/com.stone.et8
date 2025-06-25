using System;
using System.Collections.Generic;

namespace ET
{
    // 流场地图信息
    //[EntitySystemOf(typeof( ))]
    //[FriendOf(typeof( ))]
    public partial class FlowFieldMap
    {
        public int sizex = 0;                           // X轴地图实际尺寸

        public int sizey = 0;                           // Y轴地图实际尺寸

        public int cellnumx = 0;                        // X轴切分的网格数

        public int cellnumy = 0;                        // Y轴切分的网格数

        public List<FlowFieldRegion> regions = new();   // 地图所有区域

        public List<int> targetindexs = new();          // 地图目标区域ID集

        public bool islatest = false;                   // 构建状态是否为最新
    }
}
