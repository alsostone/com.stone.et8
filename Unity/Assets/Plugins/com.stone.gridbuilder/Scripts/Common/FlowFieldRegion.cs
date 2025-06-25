using System;
using System.Collections.Generic;

namespace ET
{
    // 流场切分区域类
    //[EntitySystemOf(typeof( ))]
    //[FriendOf(typeof( ))]
    public partial class FlowFieldRegion
    {
        public int index = -1;                          // 唯一索引ID(从0开始)

        public int value = -1;                          // 流场热力值(0为终点)

        public int dirindex = -1;                       // 流场方向所指的区域ID

        public bool isobstacle = false;                 // 是否为障碍区域

        public List<int> basedirindexs = new();         // 正方向连接区域ID集

        public List<int> obliqueindexs = new();         // 斜方向连接区域ID集

        public int minx = -1;                           // 左下顶点x坐标

        public int miny = -1;                           // 左下顶点y坐标

        public int maxx = -1;                           // 右上顶点x坐标

        public int maxy = -1;                           // 右上顶点y坐标

        public int CenterX()
        {
            return (minx + maxx) / 2;
        }

        public int CenterY()
        {
            return (miny + maxy) / 2;
        }
    }
}
