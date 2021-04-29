using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    private float journey;//路程
    private Vector3 old_place;//记录上一帧的位置
    private Vector3 now_place;//目前位置
    private Vector3 initial_place;//初始位置
    
    // Start is called before the first frame update
    void Start()
    {
        journey = 0;
        initial_place = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float _time=0;
        if (_time == 0)
        {
            _time += Time.deltaTime;
            if (_time > 0.1f)
            {
                _time = 0;
            }
        }
        
        now_place = transform.position;
        if (now_place != initial_place&&_time>0.1f)
        {

        }
    }
}
