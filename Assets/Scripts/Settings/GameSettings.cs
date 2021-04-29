using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    /// <summary>
    /// 模拟器数据传输方式
    /// </summary>
    public static readonly string DataType = "DataType";
    public static readonly string SceneName = "SceneName";
    public static readonly string BGMVolume = "BGMVolume";
    public static readonly string SFXVolume = "SFXVolume";
    public static readonly string Language = "Language";

    private static LanguageType mLanguageType;

    public static void LoadSettings()
    {
        //执行配置文件读取设置操作
        if (PlayerPrefs.HasKey(BGMVolume))
            AudioPlayManager.SetBGMVolume(PlayerPrefs.GetFloat(BGMVolume));
        if (PlayerPrefs.HasKey(SFXVolume))
            AudioPlayManager.SetSFXVolume(PlayerPrefs.GetFloat(SFXVolume));
        if (PlayerPrefs.HasKey(Language))
            mLanguageType = (LanguageType)PlayerPrefs.GetInt(Language);
        else
        {
            mLanguageType = LanguageType.Chinese;
            PlayerPrefs.SetInt(Language, (int)mLanguageType);
        }

        LanguageSetting.Language = mLanguageType;
    }

    /// <summary>
    /// 保存音量大小设置(范围0-1)
    /// </summary>
    /// <param name="bmgVolume">背景音乐音量</param>
    /// <param name="sfxVolume">音效音量</param>
    public static void SaveVolume(float bmgVolume, float sfxVolume)
    {
        PlayerPrefs.SetFloat(BGMVolume, Mathf.Clamp01(bmgVolume));
        PlayerPrefs.SetFloat(SFXVolume, Mathf.Clamp01(sfxVolume));
    }

    public static void SaveLanguage()
    {
        //执行配置文件存储操作(LanguageSetting、内置QualitySettings等）

        //切换语言操作，从dropdown下拉框读取种类
        mLanguageType = LanguageType.English;

        //之后迁移到MenuSettingManager脚本
        if (mLanguageType != LanguageSetting.Language)
        {
            //执行语言切换事件
            LanguageSetting.OnLanguageChanged?.Invoke();
        }
    }
}
