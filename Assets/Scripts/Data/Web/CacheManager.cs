using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CacheManager : MonoBehaviour
{
    //8位密钥
    string key = "FFXXSSDD";
    //缓存目录
    string cachePath = "";

    private static CacheManager mInstance;
    public static CacheManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject root = new GameObject("_CacheManager");
                mInstance = root.AddComponent<CacheManager>();
            }
            return mInstance;
        }
    }

    //TODO:应用结束时(断网或无法连接业务服务器时)，调用加密算法缓存信息
    public void EncryptCache(string test)
    {
        string encrypt_data = Des.Encrypt(test, key);
        Debug.Log(encrypt_data);

        string decrypt_data = Des.Decrypt(encrypt_data, key);
        Debug.Log(decrypt_data);
    }

    private void Start()
    {
        //Test
        //EncryptCache("asdasd");
    }
}
