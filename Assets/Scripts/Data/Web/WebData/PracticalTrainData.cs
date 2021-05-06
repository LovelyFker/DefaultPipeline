using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 实操（科二、科三）训练数据，发送
/// </summary>
public class PracticalTrainData : WebDataBase
{
    [Serializable]
    public struct PracticalTrainStruct
    {
        /// <summary>
        /// 训练车型，表示学员训练的车型,C1,C2...
        /// </summary>
        public string TrainType { get; set; }
        /// <summary>
        /// 训练科目，表示学员训练的科目，2:科目二, 3:科目三。
        /// </summary>
        public int Subject { get; set; }
        /// <summary>
        /// 开始时间，表示一次训练的开始时间(unix timestamp)
        /// </summary>
        public long TrainStartTicks { get; set; }
        /// <summary>
        /// 结束时间，表示一次训练的结束时间(unix timestamp)
        /// </summary>
        public long TrainEndTicks { get; set; }
        /// <summary>
        /// 行驶里程，表示一次训练的行驶里程（单位：米）
        /// </summary>
        public int TrainMileage { get; set; }
        /// <summary>
        /// 训练项目，表示一次训练的明细项目。
        /// </summary>
        public List<DetailStruct> Details { get; set; }
    }

    public struct DetailStruct
    {
        /// <summary>
        /// 训练项目，表示训练项目编码。
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 扣分数，选填。
        /// </summary>
        public int Deduct { get; set; }
        /// <summary>
        /// 开始时间，表示项目训练的开始时间(unix timestamp)
        /// </summary>
        public long TrainStartTicks { get; set; }
        /// <summary>
        /// 结束时间，表示项目训练的结束时间(unix timestamp)
        /// </summary>
        public long TrainEndTicks { get; set; }
    }

    public PracticalTrainStruct data = new PracticalTrainStruct();

    public PracticalTrainData (string url, string referer) : base(url, referer)
    {
        Httpurl = url;
        HttpReferer = referer;
    }
}
