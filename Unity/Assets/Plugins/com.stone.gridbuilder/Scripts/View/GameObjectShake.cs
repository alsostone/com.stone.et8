using UnityEngine;
using System.Collections;

public enum ShakeType
{
    Position,   // 位置震动
    Rotation,   // 旋转震动
    Scale       // 缩放震动
}

public class GameObjectShake : MonoBehaviour
{
    [Header("基础设置")]
    [SerializeField] private ShakeType shakeType = ShakeType.Position;
    [SerializeField] private float duration = 0.7f;
    [SerializeField] private float magnitude = 0.2f;
    [SerializeField] private AnimationCurve dampingCurve = AnimationCurve.Linear(0, 1, 1, 0);
    
    [Header("高级设置")]
    [SerializeField] private bool perlinNoise = true; // 使用柏林噪声获得更自然的震动
    [SerializeField] private Vector3 axisMultiplier = Vector3.one; // 各轴震动强度比例
    
    // 原始变换值
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    
    private Coroutine shakeCoroutine;

    // 开始震动
    public void StartShake(Vector3 multiplier)
    {
        axisMultiplier = multiplier;
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            ResetTransform();
        }

        Transform transform1 = transform;
        originalPosition = transform1.localPosition;
        originalRotation = transform1.localRotation;
        originalScale = transform1.localScale;
        shakeCoroutine = StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            // 计算衰减值
            float damping = dampingCurve.Evaluate(elapsed / duration);
            
            // 生成偏移值
            Vector3 offset = CalculateOffset(damping);
            
            // 应用震动效果
            ApplyShake(offset);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // 恢复原始状态
        ResetTransform();
        shakeCoroutine = null;
    }

    // 计算震动偏移
    private Vector3 CalculateOffset(float damping)
    {
        float x, y, z;
        float time = Time.time * 20f; // 噪声采样速度
        
        if (perlinNoise)
        {
            // 使用柏林噪声获得更自然的震动
            x = (Mathf.PerlinNoise(time, 0) * 2 - 1);
            y = (Mathf.PerlinNoise(0, time) * 2 - 1);
            z = (Mathf.PerlinNoise(time, time) * 2 - 1);
        }
        else
        {
            // 随机震动
            x = Random.Range(-1f, 1f);
            y = Random.Range(-1f, 1f);
            z = Random.Range(-1f, 1f);
        }
        
        // 应用轴乘数和阻尼
        Vector3 offset = new Vector3(x, y, z);
        offset = Vector3.Scale(offset, axisMultiplier);
        offset *= magnitude * damping;
        
        return offset;
    }

    // 应用震动效果
    private void ApplyShake(Vector3 offset)
    {
        switch (shakeType)
        {
            case ShakeType.Position:
                transform.localPosition = originalPosition + offset;
                break;
                
            case ShakeType.Rotation:
                // 转换为角度震动
                Vector3 rotationOffset = offset * 30f; // 缩放因子使旋转更明显
                transform.localRotation = originalRotation * Quaternion.Euler(rotationOffset);
                break;
                
            case ShakeType.Scale:
                // 缩放震动（保持正值）
                Vector3 scaleOffset = Vector3.one + offset * 0.5f;
                transform.localScale = Vector3.Scale(originalScale, scaleOffset);
                break;
        }
    }

    // 重置变换
    private void ResetTransform()
    {
        var transform1 = transform;
        transform1.localPosition = originalPosition;
        transform1.localRotation = originalRotation;
        transform1.localScale = originalScale;
    }
}