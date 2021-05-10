using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathCell : MonoBehaviour
{
    public enum PathCellType
    {
        StartingPoint,//起点
        Straight,//直路
        Bend,//弯路
        S_Bend,//S弯路
        Crossroad,//十字路口
        T_Crossing,//T字路口
        Quarter,//直角弯道
        Destination//终点
    }

    public PathCellType Type;
    //起始连接点
    public List<Transform> PathStartJointList;
    //终止连接点
    public List<Transform> PathEndJointList;

    public Collider Trigger;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Car")))
        {
            PathGenerator.Instance.OnCarEnterPathTrigger(this.gameObject);
        }
    }
}
