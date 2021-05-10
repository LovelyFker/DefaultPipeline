using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.back * speed);
    }
}
