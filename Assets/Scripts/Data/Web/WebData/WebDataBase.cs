using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using System;
using System.IO;

[Serializable]
public class WebDataBase
{
    private string url;
    public string Url
    {
        get
        {
            return url;
        }
        set
        {
            url = value;
        }
    }

    private string Referer;
    public string HttpReferer
    {
        get
        {
            return Referer;
        }
        set
        {
            Referer = value;
        }
    }

    public WebDataBase(string url)
    {
        Url = url;
    }

    public WebDataBase(string url, string referer)
    {
        Url = url;
        HttpReferer = referer;
    }

    public string StringData;

    //Http发送Get请求获取Json字符串转Struct
    public void JsonToStruct<T> (ref T data)
    {
        try
        {
            //FIXED: 考虑是否多线程处理收发数据
            data = JsonConvert.DeserializeObject<T>(HttpRequest.Get(Url));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    //本地解析Json测试
    public void JsonToStructLocal<T> (ref T data)
    {
        try
        {
            string jsonString = File.ReadAllText(Url);
            data = JsonConvert.DeserializeObject<T>(jsonString);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }


    //本地Struct转Json字符串
    public void StructToJson<T> (T data)
    {
        try
        {
            StringData = JsonConvert.SerializeObject(data);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    //向服务器Post数据请求
    public void SendData ()
    {
        if (!string.IsNullOrEmpty(StringData))
        {
            try
            {
                //FIXED: 考虑是否多线程处理收发数据
                HttpRequest.Post(Url, StringData, HttpReferer);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public void ShowData<T> (T data)
    {
        PropertyInfo[] _propertyInfos = typeof(T).GetProperties();
        //FieldInfo[] _fieldInfos = typeof(T).GetFields()

        foreach (PropertyInfo info in _propertyInfos)
        {
            Debug.Log(info.Name + " : " + typeof(T).GetProperty(info.Name).GetValue(data));
        }
    }
}
