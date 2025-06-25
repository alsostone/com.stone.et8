using System;
using System.Collections.Generic;

namespace ET
{
    // 流场算法类
    //[EntitySystemOf(typeof())]
    //[FriendOf(typeof())]
    public partial class FlowFieldSystem
    {
        // 地图信息集
        public Dictionary<string, FlowFieldMap> _mapdata = new Dictionary<string, FlowFieldMap>();

        // 初始化地图数据(mapid：地图ID名 sizex：地图X轴尺寸 sizey：地图Y轴尺寸 cellnumx：X轴切分的网格数 cellnumy：Y轴切分的网格数)
        public bool InitMap(string mapid, int sizex, int sizey, int cellnumx, int cellnumy)
        {
            if (_mapdata.ContainsKey(mapid))
                return false;

            if (sizex <= 0 || sizey <= 0 || cellnumx <= 0 || cellnumy <= 0)
                return false;

            _mapdata[mapid] = new FlowFieldMap();
            _mapdata[mapid].sizex = sizex;
            _mapdata[mapid].sizey = sizey;
            _mapdata[mapid].cellnumx = cellnumx;
            _mapdata[mapid].cellnumy = cellnumy;

            int cellcount = cellnumx * cellnumy;
            int cellwidth = sizex / cellnumx;
            int cellheight = sizey / cellnumy;
            for (int i = 0; i < cellcount; i++)
            {
                FlowFieldRegion region = new FlowFieldRegion();
                region.index = i;

                if (i % cellnumx != 0)
                    region.basedirindexs.Add(i - 1);

                if (i % cellnumx != cellnumx - 1)
                    region.basedirindexs.Add(i + 1);

                if (i < cellcount - cellnumx)
                    region.basedirindexs.Add(i + cellnumx);

                if (i >= cellnumx)
                    region.basedirindexs.Add(i - cellnumx);

                if ((i % cellnumx != 0) && (i < cellcount - cellnumx))
                    region.obliqueindexs.Add(i + cellnumx - 1);

                if ((i % cellnumx != cellnumx - 1) && (i < cellcount - cellnumx))
                    region.obliqueindexs.Add(i + cellnumx + 1);

                if ((i % cellnumx != 0) && (i >= cellnumx))
                    region.obliqueindexs.Add(i - cellnumx - 1);

                if ((i % cellnumx != cellnumx - 1) && (i >= cellnumx))
                    region.obliqueindexs.Add(i - cellnumx + 1);

                int celly = i / cellnumx;
                int cellx = i % cellnumx;
                region.minx = cellx * cellwidth;
                region.maxx = (cellx + 1) * cellwidth - 1;
                region.miny = celly * cellheight;
                region.maxy = (celly + 1) * cellheight - 1;

                _mapdata[mapid].regions.Add(region);
            }

            return true;
        }

        // 设置为目标区域(mapid：地图ID名 index：区域ID istarget：是否设为目标)
        public bool SetTargetRegion(string mapid, int index, bool istarget)
        {
            if (!_mapdata.ContainsKey(mapid))
                return false;

            var region = this.GetRegion(mapid, index);
            if (region == null || region.isobstacle)
                return false;

            var isexist = _mapdata[mapid].targetindexs.Contains(index);
            if (istarget && !isexist)
            {
                _mapdata[mapid].targetindexs.Add(index);
                _mapdata[mapid].islatest = false;
            }
            else if (!istarget && isexist)
            {
                _mapdata[mapid].targetindexs.Remove(index);
                _mapdata[mapid].islatest = false;
            }

            return true;
        }

        // 是否为目标区域(mapid：地图ID名 index：区域ID)
        public bool IsTargetRegion(string mapid, int index)
        {
            if (!_mapdata.ContainsKey(mapid))
                return false;

            return _mapdata[mapid].targetindexs.Contains(index);
        }

        // 设置障碍区域(mapid：地图ID名 index：区域ID isobstacle：是否设为障碍)
        public bool SetObstacleRegion(string mapid, int index, bool isobstacle)
        {
            if (!_mapdata.ContainsKey(mapid))
                return false;

            var region = this.GetRegion(mapid, index);
            if (region == null || region.value == 0)
                return false;

            if (!region.isobstacle && isobstacle)
            {
                region.value = -1;
                region.dirindex = -1;
                region.isobstacle = isobstacle;
                _mapdata[mapid].islatest = false;
            }
            else if (region.isobstacle && !isobstacle)
            {
                region.isobstacle = isobstacle;
                _mapdata[mapid].islatest = false;
            }

            return true;
        }

        // 是否为障碍区域(mapid：地图ID名 index：区域ID)
        public bool IsObstacleRegion(string mapid, int index)
        {
            if (!_mapdata.ContainsKey(mapid))
                return false;

            var region = this.GetRegion(mapid, index);
            if (region == null)
                return false;

            return region.isobstacle;
        }

        // 构建地图流场方向(mapid：地图ID名)
        public void BuildMap(string mapid)
        {
            if (!_mapdata.ContainsKey(mapid) || _mapdata[mapid].islatest)
                return;

            foreach (FlowFieldRegion region in _mapdata[mapid].regions)
            {
                region.value = (_mapdata[mapid].targetindexs.Contains(region.index) ? 0 : -1);
                region.dirindex = -1;
            }

            if (_mapdata[mapid].targetindexs.Count == 0)
                return;

            List<int> heatqueue = new List<int>();
            foreach (int index in _mapdata[mapid].targetindexs)
            {
                heatqueue.Add(index);
            }
            while (heatqueue.Count > 0)
            {
                var region = this.GetRegion(mapid, heatqueue[0]);
                if (region != null)
                {
                    foreach (var linkindex in region.basedirindexs)
                    {
                        var linkregion = this.GetRegion(mapid, linkindex);
                        if (linkregion != null && !linkregion.isobstacle && linkregion.value < 0)
                        {
                            linkregion.value = region.value + 1;
                            heatqueue.Add(linkindex);
                        }
                    }

                    foreach (var linkindex in region.obliqueindexs)
                    {
                        var linkregion = this.GetRegion(mapid, linkindex);
                        if (linkregion != null && !linkregion.isobstacle && linkregion.value < 0)
                        {
                            // 如果这个斜方向相邻的两个正方向格子存在障碍，则此为拐角不算作通路
                            bool iscorner = false;
                            foreach (var tempindex in linkregion.basedirindexs)
                            {
                                if (region.basedirindexs.Contains(tempindex) && this.IsObstacleRegion(mapid, tempindex))
                                {
                                    iscorner = true;
                                    break;
                                }
                            }
                            if (iscorner) continue;

                            linkregion.value = region.value + 2;
                            heatqueue.Add(linkindex);
                        }
                    }
                }
                heatqueue.RemoveAt(0);
            }

            foreach (var region in _mapdata[mapid].regions)
            {
                if (region.value == -1)
                    continue;

                int minvalue = region.value;
                foreach (var linkindex in region.basedirindexs)
                {
                    var linkregion = this.GetRegion(mapid, linkindex);
                    if (linkregion != null && linkregion.value >= 0 && linkregion.value <= minvalue)
                    {
                        region.dirindex = linkindex;
                        minvalue = linkregion.value;
                    }
                }
                foreach (var linkindex in region.obliqueindexs)
                {
                    var linkregion = this.GetRegion(mapid, linkindex);
                    if (linkregion != null && linkregion.value >= 0 && linkregion.value <= minvalue)
                    {
                        // 如果这个斜方向相邻的两个正方向格子存在障碍，则此为拐角不算作通路
                        bool iscorner = false;
                        foreach (var tempindex in linkregion.basedirindexs)
                        {
                            if (region.basedirindexs.Contains(tempindex) && this.IsObstacleRegion(mapid, tempindex))
                            {
                                iscorner = true;
                                break;
                            }
                        }
                        if (iscorner) continue;

                        region.dirindex = linkindex;
                        minvalue = linkregion.value;
                    }
                }
            }

            _mapdata[mapid].islatest = true;
        }

        // 删除地图(mapid：地图ID名)
        public void RemoveMap(string mapid)
        {
            _mapdata.Remove(mapid);
        }

        // 获取格子坐标所在区域ID(mapid：地图ID名 cellx：X轴网格坐标 celly：Y轴网格坐标)
        public int GetRegionIndex(string mapid, int cellx, int celly)
        {
            if (!_mapdata.ContainsKey(mapid))
                return -1;

            return (celly * _mapdata[mapid].cellnumx + cellx);
        }

        // 获取地图坐标所在区域ID(mapid：地图ID名 x：X轴地图坐标 y：Y轴地图坐标)
        public int GetRegionIndexByPosition(string mapid, int x, int y)
        {
            if (!_mapdata.ContainsKey(mapid))
                return -1;

            int cellwidth = _mapdata[mapid].sizex / _mapdata[mapid].cellnumx;
            int cellheight = _mapdata[mapid].sizey / _mapdata[mapid].cellnumy;
            int cellx = x / cellwidth;
            int celly = y / cellheight;

            return this.GetRegionIndex(mapid, cellx, celly);
        }

        // 获取地图信息(mapid：地图ID名)
        public FlowFieldMap GetMap(string mapid)
        {
            if (!_mapdata.ContainsKey(mapid))
                return null;

            return _mapdata[mapid];
        }

        // 获取指定ID区域(mapid：地图ID名 index：区域ID)
        public FlowFieldRegion GetRegion(string mapid, int index)
        {
            if (!_mapdata.ContainsKey(mapid))
                return null;

            if (index < 0 || index >= _mapdata[mapid].regions.Count)
                return null;

            return _mapdata[mapid].regions[index];
        }

        // 查询流向路径(mapid：地图ID名 posx：X坐标 posy：Y坐标 pathcount：查询连续路径区域的数量 regions：连续路径区域信息)
        public bool QueryPath(string mapid, int posx, int posy, int pathcount, ref List<FlowFieldRegion> regions)
        {
            int index = this.GetRegionIndexByPosition(mapid, posx, posy);
            var region = this.GetRegion(mapid, index);
            if (region == null)
                return false;

            while (region.value > 0)
            {
                region = this.GetRegion(mapid, region.dirindex);
                if (region == null)
                    break;

                regions.Add(region);

                if (pathcount > 0 && regions.Count == pathcount)
                    break;
            }

            return true;
        }
    }
}
