using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyCodeReceiveTest : MonoBehaviour
{
    public Text mText;
    public List<string> KeyCodeInfo = new List<string>();
    [Range(10, 20)]
    public int mMaxShowElement = 15;

    private int mCurrentIndex = 0;

    private void Start()
    {
        mCurrentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        mText.text = "";

        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    if (KeyCodeInfo.Count < mMaxShowElement)
                        KeyCodeInfo.Add(keyCode.ToString());
                    else
                    {
                        KeyCodeInfo.RemoveAt(0);
                        KeyCodeInfo.Add(keyCode.ToString());
                    }
                }
            }
        }

        foreach (string s in KeyCodeInfo)
        {
            mText.text += s + "\n";
        }
    }
}
