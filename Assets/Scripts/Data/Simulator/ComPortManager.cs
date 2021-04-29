using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

public class ComPortManager : MonoBehaviour
{
    private static ComPortManager mInstance;
    public static ComPortManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject root = new GameObject("_ComPortManager");
                mInstance = root.AddComponent<ComPortManager>();
            }
            return mInstance;
        }
    }

    #region 定义串口属性
    //定义基本信息
    public string portName = "COM1";//串口名
    public int baudRate = 19200;//波特率
    public Parity parity = Parity.None;//效验位
    public int dataBits = 8;//数据位
    public StopBits stopBits = StopBits.One;//停止位
    SerialPort sp = null;
    Thread dataReceiveThread;
    Thread dataSendThread;
    //发送的消息
    string message = "";
    public List<byte> listReceive = new List<byte>();
    char[] strchar = new char[100];//接收的字符信息转换为字符数组信息
    string str;
    #endregion

    public DataFromSimulator dataFromSimulator = new DataFromSimulator();
    public DataToSimulator dataToSimulator = new DataToSimulator();

    private void Awake()
    {
        if (mInstance == null)
            mInstance = this;
    }

    void Start()
    {
        OpenPort();
        dataReceiveThread = new Thread(new ThreadStart(DataReceiveFunction));
        dataReceiveThread.Start();
        //dataSendThread = new Thread(new ThreadStart(DataSendFunction));
        //dataSendThread.Start();
    }

    void Update()
    {

    }

    #region 创建串口，并打开串口
    public void OpenPort()
    {
        //创建串口
        sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        sp.ReadTimeout = 400;
        try
        {
            sp.Open();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion



    #region 程序退出时关闭串口
    void OnApplicationQuit()
    {
        ClosePort();
    }

    private void OnDestroy()
    {
        ClosePort();
    }

    public void ClosePort()
    {
        try
        {
            sp.Close();
            dataReceiveThread.Abort();
            //dataSendThread.Abort();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion


    /// <summary>
    /// 打印接收的信息
    /// </summary>
    void PrintData()
    {
        for (int i = 0; i < listReceive.Count; i++)
        {
            strchar[i] = (char)(listReceive[i]);
            str = new string(strchar);
        }
        Debug.Log(str);
    }

    #region 接收数据
    void DataReceiveFunction()
    {
        #region 按单个字节发送处理信息，不能接收中文
        //while (sp != null && sp.IsOpen)
        //{
        //    Thread.Sleep(1);
        //    try
        //    {
        //        byte addr = Convert.ToByte(sp.ReadByte());
        //        sp.DiscardInBuffer();
        //        listReceive.Add(addr);
        //        PrintData();
        //    }
        //    catch
        //    {
        //        //listReceive.Clear();
        //    }
        //}
        #endregion

        #region 按字节数组发送处理信息，信息缺失
        byte[] buffer = new byte[1024];
        while (true)
        {
            if (sp != null && sp.IsOpen)
            {
                dataFromSimulator.ComPortReceiveData(sp, buffer);
            }
            
            Thread.Sleep(10);
        }
        #endregion
    }

    void DataSendFunction()
    {
        while (true)
        {
            if (sp != null && sp.IsOpen)
            {
                dataToSimulator.ComPortSendData(sp);
            }

            Thread.Sleep(80);
        }
    }
    #endregion



    #region 发送数据
    public void WriteData(string dataStr)
    {
        if (sp.IsOpen)
        {
            sp.Write(dataStr);
        }
    }

    /*void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 29;
        myStyle.fontStyle = FontStyle.Bold;
        myStyle.normal.textColor = Color.magenta;

        message = GUILayout.TextField(message);
        if (GUILayout.Button("Send Input"))
        {
            WriteData(message);
        }
        string test = "AA BB 01 12345 01AB 0@ab 发送";//测试字符串
        if (GUILayout.Button("Send Test"))
        {
            WriteData(test);
        }

        GUI.Label(new Rect(100, 100, 100, 50), sp.IsOpen.ToString(), myStyle);
    }*/
    #endregion
}