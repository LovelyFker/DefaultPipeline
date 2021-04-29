using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum LanguageType
{
    Chinese,
    English,
    Janpanese,
    Korean,
    French,
    Germany,
    Russian
}

public static class LanguageSetting
{
    public static Dictionary<string, string> uiTextDic = new Dictionary<string, string>();

    /// <summary>
    /// 语种文件路径
    /// </summary>
    private static Dictionary<LanguageType, string> pathDic = new Dictionary<LanguageType, string>
    {
        {LanguageType.Chinese, "/Resources/Text/CN/"},
        {LanguageType.English, "/Resources/Text/EN/"},
        {LanguageType.Janpanese, "/Resources/Text/JP/"},
        {LanguageType.Korean, "/Resources/Text/KR/"},
        {LanguageType.French, "/Resources/Text/FR/"},
        {LanguageType.Germany, "/Resources/Text/DE/"},
        {LanguageType.Russian, "/Resources/Text/RU/"},
    };

    /// <summary>
    /// 全局语言切换事件，注册当前场景的所有待切换文本
    /// </summary>
    public static Action OnLanguageChanged;

    /// <summary>
    /// 添加配置存档后，从配置存档读取语言设置
    /// </summary>
    public static LanguageType Language;

    /// <summary>
    /// 若需要，3DUI语种切换
    /// </summary>
    public static void Get3DUILanguage()
    {

    }

    /// <summary>
    /// 初始化读取替换文本信息
    /// </summary>
    public static void InitializeLoadTextInfos()
    {
        ReadTexts(pathDic[Language]);
    }

    private static void ReadTexts(string path)
    {
        //确定不同字典的文件路径(待确定存储方式)
        string uiTextPath = path + "uiText.txt";

        //根据文本存储方式，读取文本信息到字典中(可考虑文本加密存储)
        uiTextDic.Add("0", "0");
    }

    public static string GetText (string keyinfo, Dictionary<string, string> textDic)
    {
        if (!string.IsNullOrEmpty(keyinfo))
        {
            string text;
            if (textDic.TryGetValue(keyinfo, out text))
            {
                if (!string.IsNullOrEmpty(text))
                    return "文本信息为空或不存在";
                else
                    return text;
            }
            else
                return "找不到文本描述对应的文本信息";
        }
        else
            return "文本描述为空或不存在";
    }
}
