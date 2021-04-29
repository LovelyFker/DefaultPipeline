using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

/// <summary>
/// 网络状态
/// </summary>
public class NetWorkState
{
    [DllImport("wininet.dll")]
    private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

    /// <summary>
    /// 获取网络连接状态
    /// </summary>
    /// <param name="url">Http协议请求接口</param>
    /// <returns></returns>
    public static int GetNetWorkState(string url)
    {
        //网络状态描述值
        int description;
        int netState = 0;

        if (!InternetGetConnectedState(out description, 0))
        {
            //无网络 
            netState = 1;
        }
        else if ((description & 1) != 0)
        {
            //调制解调器联网
            if (HttpRequest.IsCanConnect(url))
                netState = 2;
            else
                netState = 3;
        }
        else if ((description & 2) != 0)
        {
            //网卡联网
            if (HttpRequest.IsCanConnect(url))
                netState = 4;
            else
                netState = 5;
        }

        if (netState == 2 || netState == 4)
        {
            Debug.Log("连接上互联网, 能访问" + url);
        }
        else if (netState == 3 || netState == 5)
        {
            Debug.Log("连接上互联网, 不能访问" + url);
        }
        else
        {
            Debug.Log("未连接上互联网");
        }
            
        return netState;
    }
}
