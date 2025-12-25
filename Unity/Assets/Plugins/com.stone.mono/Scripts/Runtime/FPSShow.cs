using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSShow : MonoBehaviour
{
    public Text FPS;
    
    private Dictionary<int, string> fpsDic;
    private int mFrame;
    private float mElapse;

    // Start is called before the first frame update
    void Start()
    {
#if !ENABLE_DEBUG
        GameObject.Destroy(gameObject);
#else
        fpsDic = new Dictionary<int, string>();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        mElapse += Time.unscaledDeltaTime;
        mFrame++;

        if (mElapse >= 1) {
            FPS.text = GetString(mFrame);
            mElapse -= 1;
            mFrame = 0;
        }
    }

    private string GetString(int fps)
    {
        if (fpsDic.TryGetValue(fps, out string s))
            return s;
        s = $"{fps}FPS";
        this.fpsDic.Add(fps, s);
        return s;
    }
}
