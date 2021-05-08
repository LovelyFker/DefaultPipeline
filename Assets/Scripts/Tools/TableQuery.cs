using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System;

namespace Table
{
    public static class TableQuery
    {
        static string JsonPath = "Assets/Resources/Json/";

        /// <summary>
        /// 根据Index序号查询某条Json数据
        /// </summary>
        /// <typeparam name="T">Json数据实体类</typeparam>
        /// <param name="index">序号KeyValue</param>
        /// <returns>所查询的数据条目(本地Struct)</returns>
        public static T QueryJson<T>(int index)
        {
            var fileName = JsonPath + typeof(T).ToString().Substring(6, typeof(T).ToString().Length - 6) + ".json";
            string jsonString = File.ReadAllText(fileName);
            List<T> jsonStringList = JsonConvert.DeserializeObject<List<T>>(jsonString);

            var data = jsonStringList.Where(i => int.Parse(typeof(T).GetField("Index").GetValue(i).ToString()) == index).FirstOrDefault();
            return data;
        }
    }
}
