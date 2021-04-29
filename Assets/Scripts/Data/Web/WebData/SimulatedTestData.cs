using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 模拟考试(科二、科三)数据，发送
/// </summary>
public class SimulatedTestData : WebDataBase
{
    [Serializable]
    public struct SimulatedStruct
    {
        /// <summary>
        /// 考场编号，表示学员训练的考场编号。
        /// </summary>
        public string ExamRoom { get; set; }
        /// <summary>
        /// 考试车型，表示学员模考训练的车型,C1,C2...
        /// </summary>
        public string TrainType { get; set; }
        /// <summary>
        /// 考试科目，表示学员模考的科目，2:科目二, 3:科目三。
        /// </summary>
        public int Subject { get; set; }
        /// <summary>
        /// 开始时间，表示一次模考的开始时间(unix timestamp)
        /// </summary>
        public long TrainStartTicks { get; set; }
        /// <summary>
        /// 结束时间，表示一次模拟的结束时间(unix timestamp)
        /// </summary>
        public long TrainEndTicks { get; set; }
        /// <summary>
        /// 行驶里程，表示一次模考的行驶里程（单位：米）。
        /// </summary>
        public int TrainMileage { get; set; }
        /// <summary>
        /// 考核分数，表示一次模考的分数。
        /// </summary>
        public int Scores { get; set; }
        /// <summary>
        /// 是否通过，1:通过, 2:未通过。
        /// </summary>
        public int IsPassed { get; set; }
        /// <summary>
        /// 训练明细，表示一次训练的扣分项明细数据。
        /// </summary>
        public List<DetailStruct> Details { get; set; }
    }

    public struct DetailStruct
    {
        /// <summary>
        /// 训练项目，表示训练项目编码，详情参见训练项目对照表。
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 扣分数，选填。
        /// </summary>
        public int Deduct { get; set; }
    }

    public SimulatedStruct data;

    public SimulatedTestData (string url, string referer) : base(url, referer)
    {
        Httpurl = url;
        HttpReferer = referer;
    }
}
