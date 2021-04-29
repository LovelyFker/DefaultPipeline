using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using Table;

public class DataShowTest : MonoBehaviour
{
    public DataFromSimulator.ComInputData comInputData;
    public Text mText;

    public TrainInfoData trainInfoData = new TrainInfoData("url", "referer");

    // Start is called before the first frame update
    void Start()
    {
        //Json转换及查询测试
        /*trainInfoData.data.Serial = "0038";
        trainInfoData.data.TrainData = "培训数据";
        trainInfoData.data.TrainDataType = 1;
        trainInfoData.data.TrainStartTicks = 129800;
        trainInfoData.data.TrainEndTicks = 293946;
        trainInfoData.data.TrainValue = 1;

        trainInfoData.ShowData(trainInfoData.data);
        trainInfoData.StructToJson(trainInfoData.data);
        Debug.Log(trainInfoData.StringData);
        Debug.Log(Application.dataPath);

        var data = TableQuery.QueryJson<PracticalTrainDetail>(1);
        Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(data));*/

        NetWorkState.GetNetWorkState("http://www.google.com/");
        NetWorkState.GetNetWorkState("http://www.baidu.com/");
    }

    // Update is called once per frame
    void Update()
    {
        if (ApplicationManager.Instance.mDataType != SimulatorDataType.Com)
            return;

        comInputData = ComPortManager.Instance.dataFromSimulator.ComInput;
        mText.text = "";
        FieldInfo[] _fieldInfos = typeof(DataFromSimulator.ComInputData).GetFields();
        
        foreach(FieldInfo info in _fieldInfos)
        {
            mText.text += info.Name + ": ";
            mText.text += typeof(DataFromSimulator.ComInputData).GetField(info.Name).GetValue(comInputData);
            mText.text += "\n";
        }
    }
}
