using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILanguage : MonoBehaviour
{
    [Header("文本描述信息")]
    public string keyInfo;

    void OnAwake()
    {
        //注册
        LanguageSetting.OnLanguageChanged += OnLanguageChanged;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        //注销
        LanguageSetting.OnLanguageChanged -= OnLanguageChanged;
    }

    void OnLanguageChanged()
    {

    }
}
