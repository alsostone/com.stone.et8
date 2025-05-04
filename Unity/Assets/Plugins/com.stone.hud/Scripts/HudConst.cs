namespace ST.HUD
{
    public static class HudConst
    {
        // 最大Indirect Instancing渲染数量
        public const int MaxRenderCount = 10240;
        
        // 纹理数组的数量 设置时考虑名字重复率，和maxRenderCount成正比即可
        public const int MaxTextureCount = 1024;
    }
}