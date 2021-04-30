using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EncryptedCacheManager : MonoBehaviour
{
    //8位密钥
    string key = "FFXXSSDD";

    //本地应用数据加密缓存目录
    public string EncryptedCachePath
    {
        get
        {
            string path = "";
#if UNITY_EDITOR
            path = Application.dataPath + "/EncryptedCaches/"; //路径：/Assets/EncryptedCaches/
#elif UNITY_IOS
            path = Application.temporaryCachePath + "/EncryptedCaches/"; //路径：Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/Library/EncryptedCaches/
#elif UNITY_ANDROID
            path = Application.persistentDataPath + "/EncryptedCaches/"; //路径：/data/data/xxx.xxx.xxx/files/EncryptedCaches/
#else
            path = Application.dataPath + "/EncryptedCaches/"; //路径：/Assets/EncryptedCaches/
#endif
            return path;
        }
    }

    private static EncryptedCacheManager mInstance;
    public static EncryptedCacheManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject root = new GameObject("_EncryptedCacheManager");
                mInstance = root.AddComponent<EncryptedCacheManager>();
            }
            return mInstance;
        }
    }

    //TODO:应用结束时(断网或无法连接业务服务器时)，调用加密算法缓存信息
    public void EncryptCache(string test)
    {
        string encrypt_data = Des.Encrypt(test, key);
        Debug.Log(encrypt_data);

        if (!Directory.Exists(EncryptedCachePath))
            Directory.CreateDirectory(EncryptedCachePath);

        File.WriteAllBytes(EncryptedCachePath + "学员id39", System.Text.Encoding.UTF8.GetBytes(encrypt_data));

        string decrypt_data = Des.Decrypt(encrypt_data, key);
        Debug.Log(decrypt_data);
    }

    private void Start()
    {
        //Test
        EncryptCache("asdasd");
    }
}
