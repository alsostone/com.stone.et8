using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using YIUIFramework;
using Random = UnityEngine.Random;

namespace ET.Client
{
    [GM(EGMType.Common, 1, "Performance", "Pooling and NativeArray")]
    public class GM_Performance: IGMCommand
    {
        public List<GMParamInfo> GetParams()
        {
            return new()
            {
                new GMParamInfo(EGMParamType.Int, "Times") {Value = "10000"},
                new GMParamInfo(EGMParamType.Int, "Add Times") {Value = "100"},
                new GMParamInfo(EGMParamType.Int, "Random Read Times") {Value = "100"},
            };
        }
        
        public async ETTask<bool> Run(Scene clientScene, ParamVo paramVo)
        {
            var paramInt = paramVo.Get<int>(0);
            var paramAdd = paramVo.Get<int>(1);
            var paramRead = paramVo.Get<int>(2);

            float startTime = Time.realtimeSinceStartup;
            for (int i = 0; i < paramInt; i++)
            {
                var arr = ObjectPool.Instance.Fetch<List<Vector3>>();
                arr.Capacity = paramAdd;
                for (int j = 0; j < paramAdd; j++)
                {
                    arr.Add(new Vector3(j, j, j));
                }
                if (paramRead > 0)
                    for (int j = 0; j < paramRead; j++)
                    {
                        var v = arr[Random.Range(0, paramAdd)];
                    }
                arr.Clear();
                ObjectPool.Instance.Recycle(arr);
            }
            Debug.Log(("Use ObjectPool:"+ (Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
            
            startTime = Time.realtimeSinceStartup;
            for (int i = 0; i < paramInt; i++)
            {
                var arr = new NativeArray<Vector3>(paramAdd, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
                for (int j = 0; j < paramAdd; j++)
                {
                    arr[j] = new Vector3(j, j, j);
                }
                if (paramRead > 0)
                    for (int j = 0; j < paramRead; j++)
                    {
                        var v = arr[Random.Range(0, paramAdd)];
                    }
                arr.Dispose();
            }
            Debug.Log(("Use Temp NativeArray:"+ (Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
            
            startTime = Time.realtimeSinceStartup;
            for (int i = 0; i < paramInt; i++)
            {
                var arr = new NativeArray<Vector3>(paramAdd, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                for (int j = 0; j < paramAdd; j++)
                {
                    arr[j] = new Vector3(j, j, j);
                }
                if (paramRead > 0)
                    for (int j = 0; j < paramRead; j++)
                    {
                        var v = arr[Random.Range(0, paramAdd)];
                    }
                arr.Dispose();
            }
            Debug.Log(("Use TempJob NativeArray:"+ (Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
            
            startTime = Time.realtimeSinceStartup;
            for (int i = 0; i < paramInt; i++)
            {
                var arr = new NativeArray<Vector3>(paramAdd, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
                for (int j = 0; j < paramAdd; j++)
                {
                    arr[j] = new Vector3(j, j, j);
                }
                if (paramRead > 0)
                    for (int j = 0; j < paramRead; j++)
                    {
                        var v = arr[Random.Range(0, paramAdd)];
                    }
                arr.Dispose();
            }
            Debug.Log(("Use Persistent NativeArray:"+ (Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
            
            await ETTask.CompletedTask;
            return true;
        }
    }
    
}