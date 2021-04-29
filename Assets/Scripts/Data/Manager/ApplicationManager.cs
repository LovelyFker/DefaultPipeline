using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    private static ApplicationManager mInstance;
    public static ApplicationManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject root = new GameObject("_ApplicationManager");
                mInstance = root.AddComponent<ApplicationManager>();
            }
            return mInstance;
        }
    }

    private void Awake()
    {
        if (mInstance == null)
            mInstance = this;
    }

    public SimulatorDataType mDataType = SimulatorDataType.Com;

    private void Start()
    {
        if (PlayerPrefs.HasKey(GameSettings.DataType))
            mDataType = (SimulatorDataType)PlayerPrefs.GetInt(GameSettings.DataType);
        else
            PlayerPrefs.SetInt(GameSettings.DataType, (int)SimulatorDataType.Com);

        //测试
        //mDataType = SimulatorDataType.Udp;

        switch(mDataType)
        {
            case SimulatorDataType.Com:
                ComPortManager.Instance.Init();
                break;
            case SimulatorDataType.Bluetooth:
                Debug.Log("蓝牙传输");
                break;
            case SimulatorDataType.KeyCode:
                Debug.Log("手柄&模拟键盘按键");
                break;
            case SimulatorDataType.Udp:
                Debug.Log("SocketUdp协议");
                break;
            default:
                Debug.Log("Default");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
