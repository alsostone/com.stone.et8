using System;
using System.IO;
using System.Net;
using System.Text;
using ICSharpCode.SharpZipLib.BZip2;
using ProcessLog;

public class ProcessLogMgr
{
    private const int mProcessFrameLimit = 18000;
    
    private int mFrameCount = 0;
    private int mLogEnableRef = 0;
    private readonly CircleQueue<LogFrameData> mAllFrameLog;
    
    private long mSum;
    private LogFrameData mCurrentFrameLog;

    public ProcessLogMgr(int frame)
    {
        mFrameCount = frame;
        mLogEnableRef = 0;
        mAllFrameLog = new CircleQueue<LogFrameData>(300);
        
        mCurrentFrameLog = null;
        this.LogFrameBegin(frame);
    }

    public bool HasProcessLog()
    {
        if (mAllFrameLog == null) {
            return false;
        }
        return mAllFrameLog.QueueLength() > 0;
    }
    
    public void LogNextFrame()
    {
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Hash = mSum;
        }
        
        int frame = mFrameCount;
        if (frame >= mProcessFrameLimit)
        {
            mCurrentFrameLog = null;
        }
        else
        {
            mCurrentFrameLog = mAllFrameLog.GetNext();
            if (mCurrentFrameLog == null) {
                mCurrentFrameLog = new LogFrameData();
                mAllFrameLog.Enqueue(mCurrentFrameLog);
            }
            ++mFrameCount;
            
            mCurrentFrameLog.Clear();
            mCurrentFrameLog.FrameIndex = frame;
            mSum = ~frame;
        }
    }
    
    public void LogFrameBegin(int frame)
    {
        if (frame < mProcessFrameLimit)
        {
            for (int i = mFrameCount; i <= frame; i++)
            {
                var frameLog = mAllFrameLog.GetNext();
                if (frameLog == null) {
                    frameLog = new LogFrameData();
                    mAllFrameLog.Enqueue(frameLog);
                }
                frameLog.Clear();
                ++mFrameCount;
            }

            mCurrentFrameLog = mAllFrameLog.GetLast(mFrameCount - frame - 1);
            mCurrentFrameLog.Clear();
            mCurrentFrameLog.FrameIndex = frame;
            mSum = ~frame;
        }
    }

    public void LogFrameEnd()
    {
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Hash = mSum;
        }
        mCurrentFrameLog = null;
    }

    public long GetFrameHash(int frame)
    {
        var frameLog = mAllFrameLog.GetLast(mFrameCount - frame - 1);
        if (frameLog != null) {
            return frameLog.Hash;
        }
        return 0;
    }

    public void SetLogEnable(bool enable)
    {
        mLogEnableRef += enable ? 1 : -1;
    }
    
    public void SaveProcessLogToFile(string persistentPath, string folderName, string key)
    {
        var filename = new StringBuilder();
        filename.Append("process_log_");
        filename.Append(key);
        filename.Append("_").Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
        filename.Append(".bin");
        SaveLogToFile(persistentPath, folderName, filename.ToString());
    }

    public void SaveLogToFile(string persistentPath, string folderName, string filename, FileMode mode = FileMode.Create)
    {
        if (mAllFrameLog == null || mAllFrameLog.IsEmpty()) { return; }
        
        var logStream = GetLogStream();
#if ENABLE_CLIENT
        var logThread = new Thread(() =>
        {
#endif
            try {
                var path = $"{persistentPath}{folderName}";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fullname = $"{path}/{filename}";
                using (var fs = new FileStream(fullname, mode, FileAccess.Write)) {
                    BZip2.Compress(logStream, fs, false, 6);
                }
            }
            catch (Exception) {
                // ignored
            }

            logStream.Close();
#if ENABLE_CLIENT
        });
        logThread.IsBackground = true;
        logThread.Start();
#endif
    }

#if !DOTNET
    public void SaveLogToFtp(string filename, string ip, string port, string user, string password)
    {
        if (mAllFrameLog == null || mAllFrameLog.IsEmpty()) { return; }
        
        var logStream = GetLogStream();
#if ENABLE_CLIENT
        var logThread = new Thread(() =>
        {
#endif
            try {
                using var stream = new MemoryStream();
                BZip2.Compress(logStream, stream, false, 6);

                var bytes = stream.ToArray();
                if (bytes.Length > 0) {
                    var client = new WebClient();
                    client.Encoding = new UTF8Encoding(false);
                    client.Credentials = new NetworkCredential(user, password);
                    var uri = new Uri($"ftp://{ip}:{port}/{filename}");
                    client.UploadDataAsync(uri, WebRequestMethods.Ftp.UploadFile, bytes);
                }
            }
            catch (Exception) {
                // ignored
            }

            logStream.Close();
#if ENABLE_CLIENT
        });
        logThread.IsBackground = true;
        logThread.Start();
#endif
    }
#endif

    public MemoryStream GetLogStream()
    {
        var ms = new MemoryStream();
        using (var binaryWriter = new BinaryWriter(ms, new UTF8Encoding(false, true), true)) {
            var cur = mAllFrameLog.Front;
            while (cur != mAllFrameLog.Tail) {
                var logTrackFrame = mAllFrameLog.Data[cur];
                cur = (cur + 1) % mAllFrameLog.Length;
                
                binaryWriter.Write(logTrackFrame.FrameIndex);
                binaryWriter.Write(logTrackFrame.Hash);

                binaryWriter.Write(logTrackFrame.Ids.Count);
                foreach (var id in logTrackFrame.Ids)
                {
                    binaryWriter.Write(id);
                }

                binaryWriter.Write(logTrackFrame.Args.Count);
                foreach (var arg in logTrackFrame.Args)
                {
                    binaryWriter.Write(arg);
                }

                binaryWriter.Write(logTrackFrame.Strings.Count);
                foreach (var luaLog in logTrackFrame.Strings)
                {
                    var bytes = Encoding.UTF8.GetBytes(luaLog);
                    binaryWriter.Write(bytes.Length);
                    binaryWriter.Write(bytes);
                }
            }
        }
        ms.Position = 0;
        return ms;
    }

    public void LogFunction(ushort hash, long arg1, long arg2, long arg3, long arg4, long arg5, long arg6, long arg7, long arg8, long arg9, long arg10)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
            mCurrentFrameLog.Args.Add(arg2);
            mCurrentFrameLog.Args.Add(arg3);
            mCurrentFrameLog.Args.Add(arg4);
            mCurrentFrameLog.Args.Add(arg5);
            mCurrentFrameLog.Args.Add(arg6);
            mCurrentFrameLog.Args.Add(arg7);
            mCurrentFrameLog.Args.Add(arg8);
            mCurrentFrameLog.Args.Add(arg9);
            mCurrentFrameLog.Args.Add(arg10);
        }
        mSum = mSum + hash + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9 + arg10;
    }

    public void LogFunction(ushort hash, long arg1, long arg2, long arg3, long arg4, long arg5, long arg6, long arg7, long arg8, long arg9)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
            mCurrentFrameLog.Args.Add(arg2);
            mCurrentFrameLog.Args.Add(arg3);
            mCurrentFrameLog.Args.Add(arg4);
            mCurrentFrameLog.Args.Add(arg5);
            mCurrentFrameLog.Args.Add(arg6);
            mCurrentFrameLog.Args.Add(arg7);
            mCurrentFrameLog.Args.Add(arg8);
            mCurrentFrameLog.Args.Add(arg9);
        }
        mSum = mSum + hash + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8 + arg9;
    }

    public void LogFunction(ushort hash, long arg1, long arg2, long arg3, long arg4, long arg5, long arg6, long arg7, long arg8)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
            mCurrentFrameLog.Args.Add(arg2);
            mCurrentFrameLog.Args.Add(arg3);
            mCurrentFrameLog.Args.Add(arg4);
            mCurrentFrameLog.Args.Add(arg5);
            mCurrentFrameLog.Args.Add(arg6);
            mCurrentFrameLog.Args.Add(arg7);
            mCurrentFrameLog.Args.Add(arg8);
        }
        mSum = mSum + hash + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7 + arg8;
    }

    public void LogFunction(ushort hash, long arg1, long arg2, long arg3, long arg4, long arg5, long arg6, long arg7)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
            mCurrentFrameLog.Args.Add(arg2);
            mCurrentFrameLog.Args.Add(arg3);
            mCurrentFrameLog.Args.Add(arg4);
            mCurrentFrameLog.Args.Add(arg5);
            mCurrentFrameLog.Args.Add(arg6);
            mCurrentFrameLog.Args.Add(arg7);
        }
        mSum = mSum + hash + arg1 + arg2 + arg3 + arg4 + arg5 + arg6 + arg7;
    }

    public void LogFunction(ushort hash, long arg1, long arg2, long arg3, long arg4, long arg5, long arg6)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
            mCurrentFrameLog.Args.Add(arg2);
            mCurrentFrameLog.Args.Add(arg3);
            mCurrentFrameLog.Args.Add(arg4);
            mCurrentFrameLog.Args.Add(arg5);
            mCurrentFrameLog.Args.Add(arg6);
        }
        mSum = mSum + hash + arg1 + arg2 + arg3 + arg4 + arg5 + arg6;
    }

    public void LogFunction(ushort hash, long arg1, long arg2, long arg3, long arg4, long arg5)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
            mCurrentFrameLog.Args.Add(arg2);
            mCurrentFrameLog.Args.Add(arg3);
            mCurrentFrameLog.Args.Add(arg4);
            mCurrentFrameLog.Args.Add(arg5);
        }
        mSum = mSum + hash + arg1 + arg2 + arg3 + arg4 + arg5;
    }

    public void LogFunction(ushort hash, long arg1, long arg2, long arg3, long arg4)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
            mCurrentFrameLog.Args.Add(arg2);
            mCurrentFrameLog.Args.Add(arg3);
            mCurrentFrameLog.Args.Add(arg4);
        }
        mSum = mSum + hash + arg1 + arg2 + arg3 + arg4;
    }

    public void LogFunction(ushort hash, long arg1, long arg2, long arg3)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
            mCurrentFrameLog.Args.Add(arg2);
            mCurrentFrameLog.Args.Add(arg3);
        }
        mSum = mSum + hash + arg1 + arg2 + arg3;
    }

    public void LogFunction(ushort hash, long arg1, long arg2)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
            mCurrentFrameLog.Args.Add(arg2);
        }
        mSum = mSum + hash + arg1 + arg2;
    }

    public void LogFunction(ushort hash, long arg1)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
            mCurrentFrameLog.Args.Add(arg1);
        }
        mSum = mSum + hash + arg1;
    }

    public void LogFunction(ushort hash)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(hash);
        }
        mSum = mSum + hash;
    }

    public void LogString(string str)
    {
        if (mLogEnableRef < 0) return;
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(9999);
            mCurrentFrameLog.Strings.Add(str);
        }
        //mSum = mSum + str.GetHashCode();
    }
    
    public void LogStringForce(string str)
    {
        if (mCurrentFrameLog != null) {
            mCurrentFrameLog.Ids.Add(9999);
            mCurrentFrameLog.Strings.Add(str);
        }
    }
    
    // 在函数第一行调用此方法不自动添加日志
    public void LogIgnore()
    {
    }

}
