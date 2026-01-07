using TrueSync;

namespace ST.Mono
{
    public static class TSMathExtensions
    {
        private static readonly int[] permutation = { 151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        private static readonly int[] p;

        static TSMathExtensions()
        {
            p = new int[512];
            for (int i = 0; i < 256; i++)
            {
                p[i] = p[i + 256] = permutation[i];
            }
        }

        public static FP PerlinNoise(FP x, FP y)
        {
            int X = (int)FP.Floor(x) & 255;
            int Y = (int)FP.Floor(y) & 255;
        
            x -= FP.Floor(x);
            y -= FP.Floor(y);
        
            FP u = Fade(x);
            FP v = Fade(y);
        
            int A = p[X] + Y;
            int AA = p[A];
            int AB = p[A + 1];
            int B = p[X + 1] + Y;
            int BA = p[B];
            int BB = p[B + 1];

            FP x1 = Lerp(u, Grad(p[AA], x, y), Grad(p[BA], x - 1, y));
            FP x2 = Lerp(u, Grad(p[AB], x, y - 1), Grad(p[BB], x - 1, y - 1));
            FP res = Lerp(v, x1, x2);
            
            // FP.Ratio(1414, 1000)是1.414的定点表示，放大res是为了尽量和Unity的PerlinNoise效果接近
            return (res * FP.Ratio(1414, 1000) + FP.One) / FP.Two;
        }

        private static FP Fade(FP t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static FP Lerp(FP t, FP a, FP b)
        {
            return a + t * (b - a);
        }

        private static FP Grad(int hash, FP x, FP y)
        {
            int h = hash & 0xF;
            // FP u = h < 8 ? x : y;
            // FP v = h < 4 ? y : h == 12 || h == 14 ? x : 0;
            // return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
            // 等价于上面但更高效的写法
            switch (h)
            {
                case 0x0: return  x + y;
                case 0x1: return -x + y;
                case 0x2: return  x - y;
                case 0x3: return -x - y;
                case 0x4: return  x;
                case 0x5: return -x;
                case 0x6: return  x;
                case 0x7: return -x;
                case 0x8: return  y;
                case 0x9: return -y;
                case 0xA: return  y;
                case 0xB: return -y;
                case 0xC: return  y + x;
                case 0xD: return -y;
                case 0xE: return  y - x;
                case 0xF: return -y;
                default: return 0; // never happens
            }
        }
    }
}