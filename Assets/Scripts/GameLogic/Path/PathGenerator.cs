using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class PathGenerator : MonoBehaviour
{
    private static PathGenerator mInstance;
    public static PathGenerator Instance
    {
        get
        {
            if (mInstance == null)
            {
                GameObject root = new GameObject("_PathGenerator");
                mInstance = root.AddComponent<PathGenerator>();
            }
            return mInstance;
        }
    }

    public Transform PathRoot;
    public List<GameObject> PathCellPrefabs;
    public List<GameObject> PathOrigin;
    public GameObject PathDestination;
    public List<GameObject> PathQueue;
    
    public Dictionary<GameObject, int> PathDic = new Dictionary<GameObject, int>();

    //超过该数量从最初开始删除pathObj
    [Header("保留pathObj数量"), Range(20, 50)]
    public int ReservePathCount;
    [Header("终点pathObj序号"), Range(39, 46)]
    public int DestinationPathCount;

    private string prefabsPath = "Prefabs/";

    //记录当前Path序号
    private int mCurrentPathIndex = 0;
    //记录当前末尾Path序号
    private int mCurrentLastPathIndex = 0;
    //记录当前PathObj
    private GameObject mCurrentPathObj;
    //记录当前末尾PathObj
    private GameObject mCurrentLastPathObj;

    private void Awake()
    {
        if (mInstance == null)
            mInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PathOrigin.Count; i++)
        {
            GameObject pathObj;
            if (i == 0)
            {
                pathObj = Instantiate(PathOrigin[i], Vector3.zero, Quaternion.identity, PathRoot);
                PathQueue.Add(pathObj);
            }
            else
            {
                Vector3 Offset = PathOrigin[i].transform.position - PathOrigin[i].GetComponent<PathCell>().PathStartJoint.position;
                pathObj = Instantiate(PathOrigin[i], PathQueue[i - 1].GetComponent<PathCell>().PathEndJoint.position + Offset, Quaternion.identity, PathRoot);
                PathQueue.Add(pathObj);
            }

            PathDic.Add(pathObj, i);
        }

        mCurrentPathIndex = 0;
        mCurrentLastPathIndex = PathQueue.Count - 1;
        mCurrentPathObj = PathQueue[0];
        mCurrentLastPathObj = PathQueue[PathQueue.Count - 1];
    }

    // Update is called once per frame
    void Update()
    {
        //清除pathObj
        if (PathDic.Count > ReservePathCount)
        {
            var objs = from path in PathDic orderby path.Value ascending select path.Key;
            PathDic.Remove(objs.FirstOrDefault());
            Destroy(objs.FirstOrDefault());
        }
    }

    /// <summary>
    /// 车辆进入Path节点触发器
    /// </summary>
    public void OnCarEnterPathTrigger(GameObject pathObj)
    {
        int index;
        if (PathDic.TryGetValue(pathObj, out index))
        {
            //判断是否到达终点
            if (pathObj.GetComponent<PathCell>().Type == PathCell.PathCellType.Destination)
            {
                //TODO:游戏结束处理
                Debug.Log("到达终点，游戏结束");
            }

            mCurrentPathIndex = index;
            mCurrentPathObj = pathObj;
            GenerateRandomPath();
        }
        else
            Debug.LogWarning("Path字典不存在该pathObj");
    }

    /// <summary>
    /// 生成随机PathObj
    /// </summary>
    private void GenerateRandomPath()
    {
        int randomIndex = UnityEngine.Random.Range(0, PathCellPrefabs.Count);

        //判断是否该生成终点
        if (mCurrentLastPathIndex >= DestinationPathCount)
        {
            //判断是否已生成终点
            if (mCurrentLastPathObj.GetComponent<PathCell>().Type != PathCell.PathCellType.Destination)
            {
                Vector3 Offset = PathDestination.transform.position - PathDestination.GetComponent<PathCell>().PathStartJoint.position;
                GameObject pathObj = Instantiate(PathDestination, mCurrentLastPathObj.GetComponent<PathCell>().PathEndJoint.position + Offset, Quaternion.identity, PathRoot);
                mCurrentLastPathObj = pathObj;
                mCurrentLastPathIndex++;
                PathDic.Add(mCurrentLastPathObj, mCurrentLastPathIndex);
            }
        }
        else
        {
            if (mCurrentLastPathIndex - mCurrentPathIndex < 2)
            {
                Vector3 Offset = PathCellPrefabs[randomIndex].transform.position - PathCellPrefabs[randomIndex].GetComponent<PathCell>().PathStartJoint.position;
                GameObject pathObj = Instantiate(PathCellPrefabs[randomIndex], mCurrentLastPathObj.GetComponent<PathCell>().PathEndJoint.position + Offset, Quaternion.identity, PathRoot);
                mCurrentLastPathObj = pathObj;
                mCurrentLastPathIndex++;
                PathDic.Add(mCurrentLastPathObj, mCurrentLastPathIndex);
            }
        }
    }
}
