using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 启动参数数据，接收
/// </summary>
public class StartParameterData : WebDataBase
{
    [Serializable]
    public struct StartParameterStruct
    {
        /// <summary>
        /// 流水号，表示一次应用培训的唯一编号。
        /// </summary>
        public string Serial { get; set; }
        /// <summary>
        /// 应用编号，表示应用的身份ID。
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 控制类型，1:时长(分钟), 2:次(轮)数。
        /// </summary>
        public int ControlType { get; set; }
        /// <summary>
        /// 控制值，培训的最大时长或次数。
        /// </summary>
        public int ControlValue { get; set; }
        /// <summary>
        /// 参照时间，服务器时间,若应用与此时间有正负5分钟差距，请应用参照时间校准训练的数据时间。
        /// </summary>
        public long ReferTime { get; set; }
        /// <summary>
        /// 保留值，扩展用。
        /// </summary>
        public string Reserved { get; set; }
    }

    public StartParameterStruct data = new StartParameterStruct();

    public StartParameterData(string url) : base(url)
    {
        Httpurl = url;
    }
}
