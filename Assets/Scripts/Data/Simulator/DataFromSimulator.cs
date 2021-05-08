using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

/// <summary>
/// 模拟器种类，对应不同的数据传输方式
/// </summary>
public enum SimulatorDataType
{
    Udp,
    Com,
    Joystick,
    Bluetooth,
    Android_Com,
    Android_Joystick
}

[Serializable]
public class DataFromSimulator
{
    #region 当前串口模拟器
    [Serializable]
    public struct ComInputData
    {
        public bool Fire;//点火
        public bool Power;//电源
        public bool Horn;//喇叭
        [Range(0, 3)]
        public int Wiper;//雨刮器
        [Range(0, 3)]
        public int TurnLight;//转向灯
        public bool SeatBelt;//安全带
        public bool CarDoor;//车门
        public bool LowBeam;//近光灯
        public bool HighBeam;//远光灯
        [Range(0, 6)]
        public int Gear;//档位
        public float SteeringWheel;//方向盘
        public float Brake;//刹车
        public float Clutch;//离合
        public float Accelerator;//加速器、油门
        public bool HandBrake;//手刹
    }

    public ComInputData ComInput;

    /// <summary>
    /// 远光灯
    /// </summary>
    public bool IsHighBeam = false;
    /// <summary>
    /// 是否反向
    /// </summary>
    public bool IsBackOff = false;

    private float offset = 510;

    public void ComPortReceiveData(SerialPort sp, byte[] data)
    {
        int sum = sp.BytesToRead;
        try
        {
            /*if (sum == 8)
            {
                sp.Read(data, 0, sum);
            }
            if (sum == 5)
            {
                sp.Read(data, 8, 5);
                ComDataParsing(data);
            }
            if (sum == 13)
            {
                Debug.Log("读取成功");
                sp.Read(data, 0, 13);
                ComDataParsing(data);
            }*/
            sp.Read(data, 0, sum);
            ComDataParsing(data);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void ComDataParsing(byte[] data)
    {
        if (data[7] != 0x0)
        {   //启动	
            if ((data[7] & 0x10) == 0x0)
            {
                ComInput.Fire = true;
                Debug.Log("点火启动");
            }
            else
            {
                ComInput.Fire = false;
                Debug.Log("熄火"); 
            }

            if ((data[7] & 0x08) == 0x0)
            {
                ComInput.Power = true;
                Debug.Log("通电"); 
            }
            else //点火
            {
                ComInput.Power = false;
                Debug.Log("断电 如果断电了是无法点火的"); 
            }
        }

        //喇叭
        if (data[9] != 0x0)
        {
            if ((data[9] & 0x02) == 0x0)
            {
                ComInput.Horn = true;
                Debug.Log("喇叭响");
            }
            else
            {
                ComInput.Horn = false;
                Debug.Log("喇叭不响");
            }
        }
        //雨刮 0关闭、1大雨刮、2小雨刮

        if (data[8] != 0x0)
        {
            if ((data[8] & 0x04) == 0x0)
            {
                ComInput.Wiper = 1;
                Debug.Log("雨刷1档");
            }
            if ((data[8] & 0x02) == 0x0)
            {
                ComInput.Wiper = 2;
                Debug.Log("雨刷2档");
            }


            if ((data[8] & 0x80) == 0x0)//雨刮三档
            {
                ComInput.Wiper = 3;
                Debug.Log("雨刷3档");
            }
        }
        else
            ComInput.Wiper = 0;

        //转向灯 0关闭、1左转、2右转、3双闪
        if (data[7] != 0x0)
        {
            if ((data[7] & 0x02) == 0x0)
            {
                Debug.Log("右转向灯");
                ComInput.TurnLight = 2;
                if ((data[7] & 0x04) == 0x0)//双闪灯
                {
                    ComInput.TurnLight = 3;
                    Debug.Log("双闪");
                }
            }
            else
            {
                if ((data[7] & 0x04) == 0x0)
                {
                    ComInput.TurnLight = 1;
                    Debug.Log("左转向灯");
                }
                else
                {
                    ComInput.TurnLight = 0;
                    Debug.Log("关闭灯");
                }
            }

            //安全带
            if ((data[7] & 0x40) == 0x0)
            {
                ComInput.SeatBelt = true;
                Debug.Log("有安全带");
            }
            else
            {
                ComInput.SeatBelt = false;
                Debug.Log("没有安全带");
            }
            //车门
            //8.1byte 
            //    7bit:备用3    
            if ((data[7] & 0x80) == 0x0)
            {
                ComInput.CarDoor = true;
                // 车门是开的
            }
            else
            {
                ComInput.CarDoor = false;
                // 车门是关的
            }
        }


        //近光灯
        if (data[7] != 0x0)
        {
            if (IsHighBeam)
            {
                ComInput.LowBeam = false;
                Debug.Log("关近光灯");
            }
            else
            {
                if ((data[7] & 0x01) == 0x0)
                {
                    ComInput.LowBeam = true;
                    Debug.Log("开近光灯");
                }
                else
                {
                    ComInput.LowBeam = false;
                    Debug.Log("关近光灯");
                }
            }

        }
        //远光灯
        if (data[8] != 0x0)
        {
            if ((data[8] & 0x01) == 0x0)
            {
                Debug.Log("开远光灯");
                ComInput.HighBeam = true;
                IsHighBeam = true;
            }
            else
            {
                Debug.Log("关远光灯");
                ComInput.HighBeam = false;
                IsHighBeam = false;
            }

        }
        if (data[9] != 0)
        {
            //1档
            if ((data[9] & 0x40) == 0x0)
            {
                ComInput.Gear = 1;
                Debug.Log("1");
            }
            //2档
            if ((data[9] & 0x20) == 0x0)
            {
                ComInput.Gear = 2;
                Debug.Log("2档");
            }
            //3档	
            if ((data[9] & 0x80) == 0x0)
            {
                ComInput.Gear = 3;
                Debug.Log("3档");
            }
            //4档
            if ((data[9] & 0x10) == 0x0)
            {
                ComInput.Gear = 4;
                Debug.Log("4档");
            }
            //5档
            if ((data[9] & 0x01) == 0x0)
            {
                ComInput.Gear = 5;
                Debug.Log("5档");
            }
            //倒档	
            if ((data[9] & 0x08) == 0x0)
            {
                ComInput.Gear = 6;
                Debug.Log("R档");
            }
            //空档	
            bool isnull = ((data[9] & 0x40) == 0x0);
            isnull |= ((data[9] & 0x20) == 0x0);
            isnull |= ((data[9] & 0x80) == 0x0);
            isnull |= ((data[9] & 0x10) == 0x0);
            isnull |= ((data[9] & 0x01) == 0x0);
            isnull |= ((data[9] & 0x08) == 0x0);
            if (!isnull)
            {
                ComInput.Gear = 0;
                Debug.Log("空档");
            }

        }
        ///方向盘
        float caculot = (data[5] << 8) + data[6];

        if (offset > 1020)
        {
            offset = 1020;
        }
        Debug.Log(caculot + "");
        float oripoint = 1020 - offset;
        float otherpoint = oripoint - 510;
        if (caculot > oripoint)
        {
            caculot -= oripoint;
        }
        else
        {
            caculot += offset;
        }

        float middle = 510;
        float Newst = middle - caculot;
        if (Newst < 0)
        {
            Newst = -middle - Newst;
        }
        else
        {
            Newst = middle - Newst;
        }
        //   label34.Text = "newst:" + Newst;

        float asds = Newst / middle;//-1到1之间变化
        ComInput.SteeringWheel = asds;
        Debug.Log("caculot:" + asds);
        //方向盘
        //if (fromComData[6] != 0x0 || fromComData[5] != 0x0)
        //{

        //    float _buffx = 0;//((fromComData[5]<<8)&0xff00)|(0x00ff&fromComData[6]);
        //    if (fromComData[5] == 255)
        //    {

        //        _buffx = fromComData[6] - 255;

        //    }
        //    else
        //    {
        //        _buffx = fromComData[5] * 255 + fromComData[6];

        //    }
        //    if (_buffx > 0)
        //    {
        //        _buffx /= 1.8f;//txh这里原来是2.75，下面是2.3导致左右方向盘打圈转的角度不一样。
        //    }
        //    if (_buffx < 0)
        //    {
        //        _buffx /= 1.75f;//奇怪，怎么一个是2.75，一个是2.3
        //    }
        //    _buffx *= 1.2f;

        //    float steerangle = clamp(_buffx, -97.0f, 97.0f) / 97.0f;///这个是控制车轮的转向，把_buffx限制在-97和97之间，steerangle转换成-1到1之间

        //    float fangxiangpan = clamp(_buffx, -120.0f, 110.0f) / 97.0f;//这个是控制方向盘的转动角度，把_buffx限制在-120.0f和110.0f之间，调节这个可以调节实际方向盘与屏幕方向盘的同步性
        //    label23.Text = _buffx + "";
        //}

        //刹车 使用负值
        if (data[4] != 0x0)
        {
            //130、215
            int scyoumen = (data[4] + 255) % 255;//-130;
            scyoumen -= 135;
            float brakevalue = -clamp(Math.Abs(scyoumen), 0, 85) / 85.0f;
            ComInput.Brake = brakevalue;
            Debug.Log(brakevalue + "");
            Debug.Log(scyoumen + "");
            ///刹车值Mathf.Clamp是将scyoumen限制在0到85之间55
        }

        //离合
        if (data[2] != 0x0)
        {
            float clucthValue = 0;
            int lhyoumen = (data[2] + 255) % 255;//-130;
            lhyoumen -= 132;

            if (!IsBackOff)
            {
                clucthValue = clamp(lhyoumen, 0, 85) / 85.0f;
            }
            else
            {
                clucthValue = clamp(Math.Abs(lhyoumen), 0, 85) / 85.0f;
            }
            ComInput.Clutch = clucthValue;
            Debug.Log(clucthValue + "");
            Debug.Log(lhyoumen + "");
        }

        //油门 使用正值
        if (data[3] != 0x0)
        {
            int bufyoumen = (data[3] + 255) % 255;//-130;
            bufyoumen -= 130;
            float throttleValue = clamp(Math.Abs(bufyoumen), 0, 85) / 85.0f;
            ComInput.Accelerator = bufyoumen;
            Debug.Log(bufyoumen + "");
            Debug.Log(data[3] + "");
        }

        //手刹 
        if (data[9] != 0x0)
        {

            if ((data[9] & 0x04) == 0x0)
            {
                ComInput.HandBrake = true;
                Debug.Log("手刹关");
            }
            else
            {
                ComInput.HandBrake = false;
                Debug.Log("手刹开");
            }
        }
    }

    float clamp(float x, float min, float max)
    {
        if (x < min) x = min;
        if (x > max) x = max;
        return x;
    }
    #endregion

    #region 通用模拟器数据接收
    [Serializable]
    public struct SimulatorInputData
    {
        public bool Power;
        public bool LeftTurnLight;
        public bool RightTurnLight;
        public bool LowBeam; //近光灯
        public bool HighLowTransform; //交换远近灯(超车时打的灯)
        public bool SeatBelt;
        public bool HighBeam; //远光灯
        public bool Fire;
        public bool Gear1;
        public bool Gear2;
        public bool Gear3;
        public bool Gear4;
        public bool Gear5;
        public bool GearR;
        public int Gear
        {
            get
            {
                if (Gear1) return 1;
                if (Gear2) return 2;
                if (Gear3) return 3;
                if (Gear4) return 4;
                if (Gear5) return 5;
                if (GearR) return 6;
                return 0;
            }
        }
        public bool HandBrake;
        public bool EmergencyLight; //双闪应急灯
        public bool Wiper; //雨刮

        public float SteerAxis;
        public float ThrottleAxis;
        public float BrakeAxis;
        public float ClutchAxis;
    }

    public SimulatorInputData SimulatorInput;

    public readonly string SteerAxisName = "Steer";
    public readonly string ThrottleAxisName = "Throttle";
    public readonly string BrakeAxisName = "Brake";
    public readonly string ClutchAxisName = "Clutch";

    /// <summary>
    /// 获取手柄键值
    /// </summary>
    public void GetJoystickInput()
    {
        //手柄按键
        SimulatorInput.Power = Input.GetKey(KeyCode.Joystick1Button0) ? true : false;
        SimulatorInput.LeftTurnLight = Input.GetKey(KeyCode.Joystick1Button1) ? true : false;
        SimulatorInput.RightTurnLight = Input.GetKey(KeyCode.Joystick1Button2) ? true : false;
        SimulatorInput.LowBeam = Input.GetKey(KeyCode.Joystick1Button3) ? true : false;
        SimulatorInput.HighLowTransform = Input.GetKeyDown(KeyCode.Joystick1Button4) ? true : false;
        SimulatorInput.SeatBelt = Input.GetKey(KeyCode.Joystick1Button5) ? true : false;
        SimulatorInput.HighBeam = Input.GetKey(KeyCode.Joystick1Button6) ? true : false;
        SimulatorInput.Fire = Input.GetKey(KeyCode.Joystick1Button7) ? true : false;
        SimulatorInput.Gear1 = Input.GetKey(KeyCode.Joystick1Button8) ? true : false;
        SimulatorInput.Gear2 = Input.GetKey(KeyCode.Joystick1Button9) ? true : false;
        SimulatorInput.Gear3 = Input.GetKey(KeyCode.Joystick1Button10) ? true : false;
        SimulatorInput.Gear4 = Input.GetKey(KeyCode.Joystick1Button11) ? true : false;
        SimulatorInput.Gear5 = Input.GetKey(KeyCode.Joystick1Button12) ? true : false;
        SimulatorInput.GearR = Input.GetKey(KeyCode.Joystick1Button13) ? true : false;
        SimulatorInput.HandBrake = Input.GetKey(KeyCode.Joystick1Button14) ? true : false;
        SimulatorInput.EmergencyLight = Input.GetKey(KeyCode.Joystick1Button15) ? true : false;
        SimulatorInput.Wiper = Input.GetKey(KeyCode.Joystick1Button17) ? true : false;

        //手柄摇杆
        SimulatorInput.SteerAxis = Input.GetAxis(SteerAxisName);
        SimulatorInput.ThrottleAxis = Input.GetAxis(ThrottleAxisName) < 0 ? Mathf.Abs(Input.GetAxis(ThrottleAxisName)) : 0;
        SimulatorInput.BrakeAxis = Input.GetAxis(BrakeAxisName) > 0 ? Input.GetAxis(BrakeAxisName) : 0;
        SimulatorInput.ClutchAxis = Input.GetAxis(ClutchAxisName);
    }
    

    /// <summary>
    /// 获取串口数据
    /// </summary>
    public void GetComInputData(SerialPort sp, byte[] data)
    {
        int sum = sp.BytesToRead;
        try
        {
            sp.Read(data, 0, sum);
            ComInputDataParsing(data);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    /// <summary>
    /// 解析串口数据
    /// </summary>
    private void ComInputDataParsing(byte[] data)
    {

    }
    #endregion
}
