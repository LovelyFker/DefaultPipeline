using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;

public class HttpRequest
{
    public static float downLoadPercentage;
    public static Action onDownLoadCompleted;

    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string Get(string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Proxy = null;
        request.KeepAlive = false;
        request.Method = "GET";
        request.ContentType = "application/json; charset=UTF-8";//数据编码格式
        request.AutomaticDecompression = DecompressionMethods.GZip;

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
        string retString = myStreamReader.ReadToEnd();

        myStreamReader.Close();
        myResponseStream.Close();

        if (response != null)
        {
            response.Close();
        }
        if (request != null)
        {
            request.Abort();
        }

        return retString;
    }

    /// <summary>
    /// Http下载网络资源
    /// </summary>
    /// <param name="url">资源地址</param>
    /// <param name="fileType">文件扩展名</param>
    /// <param name="path">存储目录</param>
    public static void DownLoad(string url, string fileType, string path)
    {
        downLoadPercentage = 0f;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        request.Timeout = 1000;

        if (!Directory.Exists(path))
        {
            if (path != null) Directory.CreateDirectory(path);
        }

        //根据学员信息生成目录与文件名，缓存到本地
        path = $"{path}{WebDataManager.StartParameterData.data.Serial}.{fileType}";

        Stream read = response.GetResponseStream();
        Stream write = new FileStream(path, FileMode.Create);

        long totalCount = read.Length;
        long curCount = 0;

        byte[] data = new byte[1024];
        int dataCount = read.Read(data, 0, data.Length);
        while (dataCount > 0)
        {
            write.Write(data, 0, dataCount);
            curCount += dataCount;
            downLoadPercentage = curCount / totalCount;
            dataCount = read.Read(data, 0, data.Length);
        }

        read.Close();
        read.Dispose();
        write.Close();
        write.Dispose();

        onDownLoadCompleted?.Invoke();
        //注销所有委托
        onDownLoadCompleted.GetInvocationList().ToList().Clear();
    }

    /// <summary>
    /// Post请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <param name="referer"></param>
    /// <returns></returns>
    public static string Post(string url, string data, string referer)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.Referer = referer;
        //request.Proxy = new WebProxy("192.168.1.100", 80);
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        request.ContentType = "application/json; charset=UTF-8"; ;//数据编码格式
        request.ContentLength = bytes.Length;
        Stream myResponseStream = request.GetRequestStream();
        myResponseStream.Write(bytes, 0, bytes.Length);

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        string retString = myStreamReader.ReadToEnd();

        myStreamReader.Close();
        myResponseStream.Close();

        if (response != null)
        {
            response.Close();
        }
        if (request != null)
        {
            request.Abort();
        }
        return retString;
    }

    /// <summary>
    /// Http判断可否连接
    /// </summary>
    /// <param name="url">Http协议请求接口</param>
    /// <returns></returns>
    public static bool IsCanConnect(string url)
    {
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        //初始标志可连接
        bool flag = true;

        try
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            //超时设置
            request.Timeout = 100;
            response = (HttpWebResponse)request.GetResponse();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            //发生异常时，设置为无法连接
            flag = false;
        }
        finally
        {
            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
        }
        return flag;
    }
}