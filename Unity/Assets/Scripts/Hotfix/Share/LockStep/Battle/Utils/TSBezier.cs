using TrueSync;

namespace ET
{
    public static class TSBezier
    {
        /// <summary>
        /// linear curve B(t) = (1-t)p0 + tp1;
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static TSVector GetPoint(TSVector p0, TSVector p1, FP t)
        {
            FP oneMinusT = FP.One - t;
            return oneMinusT * p0 + t * p1;
        }

        public static TSVector GetFirstDerivative(TSVector p0, TSVector p1, FP t)
        {
            return p1 - p0;
        }

        /// <summary>
        /// 二次贝塞尔曲线
        /// linear curve B(t) = (1-t)p0 + tp1;
        /// step deeper
        /// B(t) = (1-t)((1-t)p0 + tp1) + t((1-t)p1 + tp2) = (1-t)^2p0 + 2(1-t)p1 + t^2p2
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static TSVector GetPoint(TSVector p0, TSVector p1, TSVector p2, FP t)
        {
            FP oneMinusT = FP.One - t;
            return oneMinusT * oneMinusT * p0 +
                    2 * oneMinusT * t * p1 +
                    t * t * p2;
        }

        /// <summary>
        /// 二次贝塞尔曲线导数
        /// B'(t) = 2(1-t)(p1-p0) + 2t(p2-p1)
        /// 切线
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static TSVector GetFirstDerivative(TSVector p0, TSVector p1, TSVector p2, FP t)
        {
            return 2 * (FP.One - t) * (p1 - p0) + 2 * t * (p2 - p1);
        }

        /// <summary>
        /// 三次贝塞尔曲线
        ///  B(t) = (1 - t)^3*P0 + 3(1 - t)^2*t*P1 + 3*(1 - t)t^2*P2 + t^3*P3
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static TSVector GetPoint(TSVector p0, TSVector p1, TSVector p2, TSVector p3, FP t)
        {
            t = TSMath.Clamp01(t);
            FP oneMinusT = FP.One - t;
            return oneMinusT * oneMinusT * oneMinusT * p0 +
                    3 * oneMinusT * oneMinusT * t * p1 +
                    3 * oneMinusT * t * t * p2 +
                    t * t * t * p3;
        }

        /// <summary>
        /// 三次贝塞尔曲线导数
        /// B'(t) = 3*(1 - t)^2*(P1 - P0) + 6*(1 - t)*t*(P2 - P1) + 3*t^2 (P3 - P2).
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static TSVector GetFirstDerivative(TSVector p0, TSVector p1, TSVector p2, TSVector p3, FP t)
        {
            t = TSMath.Clamp01(t);
            FP oneMinusT = FP.One - t;
            return 3 * oneMinusT * oneMinusT * (p1 - p0) +
                    6 * oneMinusT * t * (p2 - p1) +
                    3 * t * t * (p3 - p2);
        }
    }
}