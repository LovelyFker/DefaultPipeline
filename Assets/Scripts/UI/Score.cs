using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text ScoreText;//路程显示文本
    public static float journey;//路程
    public static float oil_mass;//油量
    public Text OilMassText;//油量文本
    public Slider OilMassStrip;//油量条
    private Vector3 now_place;//实时位置
    private Vector3 initial_place;//初始位置
    private float _time;
    
    // Start is called before the first frame update
    void Start()
    {
        oil_mass = 100;
        _time = 0;
        journey = 0;
        initial_place = transform.position;
        OilMassText.text = "油量：" + oil_mass.ToString() + "%";
    }

    // Update is called once per frame
    void Update()
    {
        now_place = transform.position;
        _time += Time.deltaTime;
        if (_time > 0.1)
        {
            journey += Vector3.Distance(now_place, initial_place)/10;
            if (oil_mass > 0)
            {
                oil_mass -= Vector3.Distance(now_place, initial_place) / 10;
            }
            else
            {
                oil_mass = 0;
            }
            initial_place = now_place;
            _time = 0;
            //Debug.Log(journey.ToString("f1") + "路程");
            ScoreText.text ="距离："+ journey.ToString("f1")+"米";
            OilMassText.text = "油量：" + oil_mass.ToString("f1") + "%";
        }
        OilMassStrip.value = oil_mass;
    }
}
