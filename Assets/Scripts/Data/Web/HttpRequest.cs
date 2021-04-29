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
    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="Url"></param>
    /// <returns></returns>
    public static string Get(string Url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
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
    /// Post请求
    /// </summary>
    /// <param name="Url"></param>
    /// <param name="Data"></param>
    /// <param name="Referer"></param>
    /// <returns></returns>
    public static string Post(string Url, string Data, string Referer)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
        request.Method = "POST";
        request.Referer = Referer;
        //request.Proxy = new WebProxy("192.168.1.100", 80);
        byte[] bytes = Encoding.UTF8.GetBytes(Data);
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