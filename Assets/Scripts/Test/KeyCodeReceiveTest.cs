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

    public readonly string SteerAxis = "Steer";
    public readonly string BrakeAxis = "Brake";
    public readonly string ClutchAxis = "Clutch";
    public readonly string ThrottleAxis = "Throttle";

    private float steerValue;
    private float brakeValue;
    private float clutchValue;
    private float throttleValue;

    float three;
    float four;
    float seven;
    float eight;
    float night;
    float ten;

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

        steerValue = Input.GetAxis(SteerAxis);
        brakeValue = Input.GetAxis(BrakeAxis);
        throttleValue = Input.GetAxis(ThrottleAxis);
        clutchValue = Input.GetAxis(ClutchAxis);

        three = Input.GetAxis("three");
        four = Input.GetAxis("four");
        seven = Input.GetAxis("seven");
        eight = Input.GetAxis("eight");
        night = Input.GetAxis("night");
        ten = Input.GetAxis("ten");
    }

    private void OnGUI()
    {
        GUIStyle mHighLightStyle = new GUIStyle();
        mHighLightStyle.fontSize = 29;
        mHighLightStyle.fontStyle = FontStyle.Bold;
        mHighLightStyle.normal.textColor = new Color(1f, 0f, 0.5f);

        GUI.Label(new Rect(100, 100, 200, 100), "steer: " + steerValue, mHighLightStyle);
        GUI.Label(new Rect(100, 150, 200, 100), "brake: " + brakeValue, mHighLightStyle);
        GUI.Label(new Rect(100, 200, 200, 100), "throttle: " + throttleValue, mHighLightStyle);
        GUI.Label(new Rect(100, 250, 200, 100), "clutch: " + clutchValue, mHighLightStyle);

        GUI.Label(new Rect(100, 300, 200, 100), "3: " + three, mHighLightStyle);
        GUI.Label(new Rect(100, 350, 200, 100), "4: " + four, mHighLightStyle);
        GUI.Label(new Rect(100, 400, 200, 100), "7: " + seven, mHighLightStyle);
        GUI.Label(new Rect(100, 450, 200, 100), "8: " + eight, mHighLightStyle);
        GUI.Label(new Rect(100, 500, 200, 100), "9: " + night, mHighLightStyle);
        GUI.Label(new Rect(100, 550, 200, 100), "10: " + ten, mHighLightStyle);
    }
}
