using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 培训协议数据，发送
/// </summary>
public class TrainInfoData : WebDataBase
{
    [Serializable]
    public struct TrainInfoStruct
    {
        /// <summary>
        /// 流水号，表示一次应用培训的唯一编号。
        /// </summary>
        public string Serial { get; set; }
        /// <summary>
        /// 培训值，表示培训的具体时长或次数。
        /// </summary>
        public int TrainValue { get; set; }
        /// <summary>
        /// 开始时间，培训的开始时间(unix timestamp)。
        /// </summary>
        public long TrainStartTicks { get; set; }
        /// <summary>
        /// 结束时间，培训的结束时间(unix timestamp)。
        /// </summary>
        public long TrainEndTicks { get; set; }
        /// <summary>
        /// 数据类型，表示培训数据的类型, 1:Json数据, 2:URL。
        /// </summary>
        public int TrainDataType { get; set; }
        /// <summary>
        /// 培训数据，Json数据或数据包URL，根据协议要求
        /// </summary>
        public string TrainData { get; set; }
    }

    public TrainInfoStruct data = new TrainInfoStruct();

    public TrainInfoData(string url, string referer) : base(url, referer)
    {
        Httpurl = url;
        HttpReferer = referer;
    }
}
