using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ExcelToJsonTool : EditorWindow
{
    private string txt_JsonSavePath = "Resources/Json";
    private string txt_EntitySavePath = "Scripts/Table";
    private Vector2 scrollPos;
    [MenuItem("Custom/ExcelToJsonTool")]
    static void OpenWindow()
    {
        ExcelToJsonTool window = (ExcelToJsonTool)EditorWindow.GetWindow(typeof(ExcelToJsonTool));
        window.Show();
    }

    private void OnGUI()
    {
        GUIStyle mHighLightStyle = new GUIStyle();
        mHighLightStyle.fontSize = 18;
        mHighLightStyle.fontStyle = FontStyle.Bold;
        mHighLightStyle.normal.textColor = new Color(1f, 0f, 0.5f);

        GUILayout.Label("Excel转Json工具", mHighLightStyle);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        if (GUILayout.Button("生成实体类"))
        {
            CreateEntitiesAll();
        }
        if (GUILayout.Button("生成Json"))
        {
            ExcelToJsonAll();
        }
        EditorGUILayout.EndScrollView();
    }

    private void ExcelToJsonAll()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/Resources/Excel");
        EditorUtility.ClearProgressBar();

        int totalExcelCount = 0;
        int curCount = 0;

        var fileInfoList = dirInfo.GetFiles();
        var excelInfoList = fileInfoList.Where(i => i.FullName.Contains(".xlsx") == true && i.FullName.Contains(".meta") == false).ToList();
        totalExcelCount = excelInfoList.Count;

        if (excelInfoList.Count < 1)
        {
            this.ShowNotification(new GUIContent("没有找到需要转换的excel表"));
            return;
        }

        foreach (FileInfo file in excelInfoList)
        {
            EditorUtility.DisplayProgressBar("ExcelDataTool", "生成Json数据中...", (float)curCount / totalExcelCount);
            curCount++;
            ExcelToJson(file.Name);
        }

        if (curCount >= totalExcelCount)
        {
            EditorUtility.ClearProgressBar();
        }
    }

    private void ExcelToJson(string excelName)
    {
        if (!string.IsNullOrEmpty(excelName))
        {
            string filepath = Application.dataPath + "/Resources/Excel/" + excelName;
            string headPath = $"{Application.dataPath}/{txt_JsonSavePath}";
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                using (ExcelPackage ep = new ExcelPackage(fs))
                {
                    //获得所有工作表
                    ExcelWorksheets workSheets = ep.Workbook.Worksheets;
                    List<System.Object> lst = new List<object>();

                    //遍历所有工作表
                    for (int i = 1; i <= workSheets.Count; i++)
                    {
                        //当前工作表 
                        ExcelWorksheet sheet = workSheets[i];
                        //初始化集合
                        lst.Clear();
                        int columnCount = sheet.Dimension.End.Column;
                        int rowCount = sheet.Dimension.End.Row;
                        //根据实体类创建对象集合序列化到json中
                        for (int z = 4; z <= rowCount; z++)
                        {
                            Assembly ab = Assembly.Load("Assembly-CSharp"); //要注意对面在那个程序集里面dll
                            Type type = ab.GetType($"Table.{sheet.Name}");
                            if (type == null)
                            {
                                Debug.LogError("你还没有创建对应的实体类!");
                                return;
                            }
                            if (!Directory.Exists(headPath))
                                Directory.CreateDirectory(headPath);
                            object o = ab.CreateInstance(type.ToString());
                            for (int j = 1; j <= columnCount; j++)
                            {
                                FieldInfo fieldInfo = type.GetField(sheet.Cells[i, j].Text); //先获得字段信息，方便获得字段类型
                                object value = Convert.ChangeType(sheet.Cells[z, j].Text, fieldInfo.FieldType);
                                type.GetField(sheet.Cells[1, j].Text).SetValue(o, value);
                            }
                            lst.Add(o);
                        }
                        //写入json文件
                        string jsonPath = $"{headPath}/{sheet.Name}.json";
                        if (!File.Exists(jsonPath))
                        {
                            File.Create(jsonPath).Dispose();
                        }
                        File.WriteAllText(jsonPath, JsonConvert.SerializeObject(lst));
                    }
                }
            }
            AssetDatabase.Refresh();
        }
    }

    private void CreateEntitiesAll()
    {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/Resources/Excel");
        EditorUtility.ClearProgressBar();

        int totalExcelCount = 0;
        int curCount = 0;

        var fileInfoList = dirInfo.GetFiles();
        var excelInfoList = fileInfoList.Where(i => i.FullName.Contains(".xlsx") == true && i.FullName.Contains(".meta") == false).ToList();
        totalExcelCount = excelInfoList.Count;

        if (excelInfoList.Count < 1)
        {
            this.ShowNotification(new GUIContent("没有找到需要转换的excel表"));
            return;
        }

        foreach (FileInfo file in excelInfoList)
        {
            EditorUtility.DisplayProgressBar("ExcelDataTool", "生成实体类中...", (float)curCount / totalExcelCount);
            curCount++;
            CreateEntities(file.Name);
        }

        if (curCount >= totalExcelCount)
        {
            EditorUtility.ClearProgressBar();
        }
    }

    private void CreateEntities(string excelName)
    {
        if (!string.IsNullOrEmpty(excelName))
        {
            string filepath = Application.dataPath + "/Resources/Excel/" + excelName;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                using (ExcelPackage ep = new ExcelPackage(fs))
                {
                    //获得所有工作表
                    ExcelWorksheets workSheets = ep.Workbook.Worksheets;
                    //遍历所有工作表
                    for (int i = 1; i <= workSheets.Count; i++)
                    {
                        CreateEntity(workSheets[i]);
                    }
                    AssetDatabase.Refresh();
                }
            }
        }
    }

    private void CreateEntity(ExcelWorksheet sheet)
    {
        string dir = $"{Application.dataPath}/{txt_EntitySavePath}";
        string path = $"{dir}/{sheet.Name}.cs";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("namespace Table");
        sb.AppendLine("{");
        sb.AppendLine($"\tpublic class {sheet.Name}");
        sb.AppendLine("\t{");
        //遍历sheet首行每个字段描述的值
        //Debug.Log("column = " + sheet.Dimension.End.Column);
        //Debug.Log("row = " + sheet.Dimension.End.Row);
        for (int i = 1; i <= sheet.Dimension.End.Column; i++)
        {
            sb.AppendLine("\t\t/// <summary>");
            sb.AppendLine($"\t\t///{sheet.Cells[3, i].Text}");
            sb.AppendLine("\t\t/// </summary>");
            sb.AppendLine($"\t\tpublic {sheet.Cells[2, i].Text} {sheet.Cells[1, i].Text};");
        }
        sb.AppendLine("\t}");
        sb.AppendLine("}");
        try
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(path))
            {
                File.Create(path).Dispose(); //避免资源占用
            }
            File.WriteAllText(path, sb.ToString());
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Excel转json时创建对应的实体类出错，实体类为：{sheet.Name},e:{e.Message}");
        }
    }
}