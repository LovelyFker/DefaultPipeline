using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class DownloadCacheManager : MonoBehaviour
{
    private static DownloadCacheManager mInstance;
    public static DownloadCacheManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject root = new GameObject("_DownloadCacheManager");
                mInstance = root.AddComponent<DownloadCacheManager>();
            }
            return mInstance;
        }
    }

    private Thread mThread;

    //下载资源缓存目录
    public string DownloadCachePath
    {
        get
        {
            string path = "";
#if UNITY_EDITOR
            path = Application.dataPath + "/DownloadCaches/"; //路径：/Assets/DownloadCaches/
#elif UNITY_IOS
            path = Application.temporaryCachePath + "/DownloadCaches/"; //路径：Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/DownloadCaches/
#elif UNITY_ANDROID
            path = Application.persistentDataPath + "/DownloadCaches/"; //路径：/data/data/xxx.xxx.xxx/files/DownloadCaches/
#else
            path = Application.dataPath + "/DownloadCaches/"; //路径：/Assets/DownloadCaches/
#endif
            return path;
        }
    }

    /// <summary>
    /// 图片缓存
    /// </summary>
    public string ImageCachePath
    {
        get
        {
            return DownloadCachePath + "/Images/";
        }
    }

    /// <summary>
    /// 音频缓存
    /// </summary>
    public string AudioCachePath
    {
        get
        {
            return DownloadCachePath + "/Audios/";
        }
    }

    /// <summary>
    /// 视频缓存
    /// </summary>
    public string VideoCachePath
    {
        get
        {
            return DownloadCachePath + "/Video/";
        }
    }

    /// <summary>
    /// 文本缓存
    /// </summary>
    public string TextCachePath
    {
        get
        {
            return DownloadCachePath + "/Text/";
        }
    }

    /// <summary>
    /// 下载缓存网络资源
    /// </summary>
    public void DownLoad(string url, string fileType, string path)
    {
        if (mThread.IsAlive)
        {
            Debug.LogError("下载线程工作中！");
            return;
        }

        mThread.Abort();    
        mThread = new Thread(() => HttpRequest.DownLoad(url, fileType, path));
        HttpRequest.onDownLoadCompleted += () => { mThread.Abort(); };
        mThread.Start();
    }

    private void OnDestroy()
    {
        if (mThread != null)
            mThread.Abort();
    }

    private void OnApplicationQuit()
    {
        if (mThread != null)
            mThread.Abort();
    }
}
