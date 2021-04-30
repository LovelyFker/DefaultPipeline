using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public SimulatorDataType mDataType = SimulatorDataType.Com;
    public Transform gControllerObj;

    private List<SimulatorControllerBase> mControllers = new List<SimulatorControllerBase>();

    private void Awake()
    {
        if (mInstance == null)
            mInstance = this;

        foreach (SimulatorControllerBase controller in gControllerObj.GetComponents<SimulatorControllerBase>())
            mControllers.Add(controller);
    }

    

    private void Start()
    {
        if (PlayerPrefs.HasKey(GameSettings.DataType))
            mDataType = (SimulatorDataType)PlayerPrefs.GetInt(GameSettings.DataType);
        else
            PlayerPrefs.SetInt(GameSettings.DataType, (int)SimulatorDataType.Com);

        //测试
        mDataType = SimulatorDataType.Joystick;

        switch(mDataType)
        {
            case SimulatorDataType.Com:
                ComPortManager.Instance.Init();
                mControllers.Where(i => i.GetType() == typeof(ComPortController)).FirstOrDefault().enabled = true;
                break;
            case SimulatorDataType.Bluetooth:
                Debug.Log("蓝牙传输");
                break;
            case SimulatorDataType.Joystick:
                JoystickManager.Instance.Init();
                mControllers.Where(i => i.GetType() == typeof(JoystickController)).FirstOrDefault().enabled = true;
                break;
            case SimulatorDataType.Udp:
                Debug.Log("SocketUdp协议");
                break;
            default:
                Debug.Log("Default");
                break;
        }
    }
}
