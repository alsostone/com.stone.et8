using ET;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 简单的窗口动画
    /// 只是一个案例 不需要可以删除
    /// 这里用到dotween动画 ettask 简单的扩展
    /// 目前已知BUG 同时有多个人调用动画 其他异步还没有完成时 被其他的删除了就会错误
    /// 导致其他异步全部中断
    /// </summary>
    public static class DOTweenAsync
    {
        //对dotween的异步扩展
        //临时方案 还不够完善
        //目前这个只是在UI动画上使用 其他地方请自行实现
        private static async ETTask GetAwaiter(this Tweener tweener)
        {
            var task = ETTask.Create();
            tweener.onKill     += () => { task.SetResult(); };
            tweener.onComplete += () => { task.SetResult(); };
            await task;
        }

        public static async ETTask DOFadeAsync(this UnityEngine.UI.Image target, float endValue, float duration)
        {
            TweenerCore<Color, Color, ColorOptions> t = DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration);
            await t.SetTarget(target);
        }
        
        
    }
}