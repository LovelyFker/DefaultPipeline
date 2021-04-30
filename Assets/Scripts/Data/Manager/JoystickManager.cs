using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoBehaviour
{
    private static JoystickManager mInstance;
    public static JoystickManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject root = new GameObject("_JoystickManager");
                mInstance = root.AddComponent<JoystickManager>();
            }
            return mInstance;
        }
    }

    public DataFromSimulator dataFromSimulator = new DataFromSimulator();

    private void Awake()
    {
        if (mInstance == null)
            mInstance = this;
    }


    public void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        dataFromSimulator.GetJoystickInput();
    }
}
