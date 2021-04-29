using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

[Serializable]
public class DataToSimulator
{
    [Serializable]
    public struct ComOutputData
    {
        public float Speed;//速度
        public float Mileage;//里程
        public float Temperature;//温度
        public float RotateSpeed;//转速
        public float Oil;//油量
        public float BackLight;//背光
        public float Brightness;//亮度
        public float Hours;//小时数
        public float Minutes;//分钟数

        ///指示灯系列
        public bool PilotLight;//指示灯
        public bool HandBrake;//手刹
        public bool Accelerator;//加油、油门
        public bool Power;//电池、电源
        public bool LeftTurnLight;//左转灯
        public bool EPC;//EPC灯
        public bool ABS;//ABS灯
        public bool EngineOil;//机油
        public bool AirBag;//安全气囊
        public bool Wiper;//喷水雨刮
        public bool RightTurnLight;//右转灯
        public bool BackFogLight;//后雾灯
        public bool FrontFogLight;//前雾灯
        public bool Engine;//引擎发动机
        public bool HighBeam;//远光灯
        public bool LowBeam;//近光灯
        public bool SeatBelt;//安全带
    }

    public ComOutputData ComOutPut;

    public void ComPortSendData(SerialPort sp)
    {
        byte[] bytes = new byte[19];

        WriteData(ref bytes);

        try
        {
            sp.Write(bytes, 0, bytes.Length);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }

    private void WriteData(ref byte[] bytes)
    {
        bytes[0] = 0xAA;
        bytes[1] = 0x01;
        bytes[2] = getdate3();//灯光
        bytes[3] = getdate4();//灯光
        bytes[4] = getdate5();//灯光
        bytes[5] = Convert.ToByte(ComOutPut.Hours);//时间/小时
        bytes[6] = 0x00;//时间/分钟
        int _mile = (int)ComOutPut.Mileage;//里程表
        byte _low1 = (byte)(_mile & 0x000000ff);
        byte _low2 = (byte)((_mile & 0x0000ff00) >> 8);
        byte _hig1 = (byte)((_mile & 0x00ff0000) >> 16);
        bytes[7] = _hig1;//总里程H1
        bytes[8] = _low2;//总里程L2
        bytes[9] = _low1;//总里程L1   
        bytes[10] = Convert.ToByte(ComOutPut.Temperature);//温度
        float speed_z = ComOutPut.RotateSpeed * 245 / 6400;
        if (speed_z < 0)
        {
            speed_z = 0;
        }
        if (speed_z > 245)
        {
            speed_z = 245;
        }
        bytes[11] = Convert.ToByte(speed_z);//转速
        bytes[12] = Convert.ToByte(ComOutPut.Oil);//油量
        float speed = ComOutPut.Speed * 255 / 220;
        if (speed < 0)
        {
            speed = 0;
        }
        if (speed > 255)
        {
            speed = 255;
        }
        bytes[13] = Convert.ToByte(speed);//速度
        bytes[14] = Convert.ToByte(ComOutPut.BackLight);//背光
        bytes[15] = Convert.ToByte(ComOutPut.Brightness);//亮度
        bytes[16] = 0x00;
        int _temp = 0;
        for (int i = 1; i < 17; i++)
        {
            _temp = _temp ^ bytes[i];
        }
        bytes[17] = (byte)_temp;
        bytes[18] = 0xbb;
    }

    byte set_bit(byte data, int index, bool flag)
    {
        if (index > 8 || index < 1)
            throw new Exception("字节错误");
        int v = index < 2 ? index : (2 << (index - 2));
        return flag ? (byte)(data | v) : (byte)(data & ~v);
    }
    byte getdate5()
    {
        byte tmpd = 0x00;
        if (ComOutPut.Engine)
        {
            tmpd = set_bit(tmpd, 1, true);
        }
        if (ComOutPut.HighBeam)
        {
            tmpd = set_bit(tmpd, 2, true);
        }
        if (ComOutPut.LowBeam)
        {
            tmpd = set_bit(tmpd, 3, true);
        }
        if (ComOutPut.SeatBelt)
        {
            tmpd = set_bit(tmpd, 4, true);
        }
        tmpd = set_bit(tmpd, 5, false);

        tmpd = set_bit(tmpd, 6, false);

        tmpd = set_bit(tmpd, 7, false);

        tmpd = set_bit(tmpd, 8, false);

        return tmpd;
    }
    byte getdate4()
    {
        byte tmpd = 0x00;
        if (ComOutPut.AirBag)
        {
            tmpd = set_bit(tmpd, 1, true);
        }
        if (ComOutPut.Wiper)
        {
            tmpd = set_bit(tmpd, 2, true);
        }
        if (ComOutPut.RightTurnLight)
        {
            tmpd = set_bit(tmpd, 3, true);
        }

        tmpd = set_bit(tmpd, 4, false);

        if (ComOutPut.BackFogLight)
        {
            tmpd = set_bit(tmpd, 5, true);
        }

        //待定
        tmpd = set_bit(tmpd, 6, false);

        if (ComOutPut.FrontFogLight)
        {
            tmpd = set_bit(tmpd, 7, true);
        }

        //待定
        tmpd = set_bit(tmpd, 8, false);

        return tmpd;
    }

    byte getdate3()
    {
        byte tmpd = 0x00;
        if (ComOutPut.PilotLight)
        {
            tmpd = set_bit(tmpd, 1, true);
        }
        if (ComOutPut.HandBrake)
        {
            tmpd = set_bit(tmpd, 2, true);
        }
        if (ComOutPut.Accelerator)
        {
            tmpd = set_bit(tmpd, 3, true);
        }
        if (ComOutPut.Power)
        {
            tmpd = set_bit(tmpd, 4, true);
        }
        if (ComOutPut.LeftTurnLight)
        {
            tmpd = set_bit(tmpd, 5, true);
        }
        if (ComOutPut.EPC)
        {
            tmpd = set_bit(tmpd, 6, true);
        }
        if (ComOutPut.ABS)
        {
            tmpd = set_bit(tmpd, 7, true);
        }
        if (ComOutPut.EngineOil)
        {
            tmpd = set_bit(tmpd, 8, true);
        }
        return tmpd;
    }
}
