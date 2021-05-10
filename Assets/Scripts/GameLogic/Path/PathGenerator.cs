using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

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
    public GameObject PathStartingPoint;
    public GameObject PathDestination;
    
    public Dictionary<GameObject, int> PathDic = new Dictionary<GameObject, int>();
    //放置错误方向的pathObj
    public Dictionary<int, GameObject> WrontPathDic = new Dictionary<int, GameObject>();

    //超过该数量从最初开始删除pathObj
    [Header("保留pathObj数量"), Range(20, 50)]
    public int ReservePathCount;
    [Header("终点pathObj序号"), Range(39, 46)]
    public int DestinationPathCount;
    [Header("当前path与lastPath的间隔数"), Range(2, 15)]
    public int CountBetweenCurAndLast;
    [Header("错误方向的path生成数"), Range(5, 10)]
    public int WrongDirectionPathCount;

    private string prefabsPath = "Prefabs/";

    //记录当前Path序号
    private int mCurrentPathIndex = 0;
    //记录当前末尾Path序号
    private int mCurrentLastPathIndex = 0;
    //记录当前PathObj
    private GameObject mCurrentPathObj;
    //记录当前末尾PathObj
    private GameObject mCurrentLastPathObj;
    //记录当前路径地块的Y轴旋转量
    private float mCurrentRotationY;

    private void Awake()
    {
        if (mInstance == null)
            mInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < CountBetweenCurAndLast + 1; i++)
        {
            if (i == 0)
            {
                GameObject pathObj = Instantiate(PathStartingPoint, Vector3.zero, Quaternion.identity, PathRoot);
                PathDic.Add(pathObj, i);
                mCurrentLastPathIndex = 0;
                mCurrentLastPathObj = pathObj;
            }
            else
            {
                GenerateRandomPath();
            }
        }

        mCurrentPathIndex = 0;
        mCurrentPathObj = PathDic.FirstOrDefault().Key;
        mCurrentRotationY = 0f;
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
        int randomIndex = Random.Range(0, PathCellPrefabs.Count);

        //判断是否该生成终点
        if (mCurrentLastPathIndex >= DestinationPathCount)
        {
            //判断是否已生成终点
            if (mCurrentLastPathObj.GetComponent<PathCell>().Type != PathCell.PathCellType.Destination)
                GeneratePath(PathDestination);
        }
        else
        {
            if (mCurrentLastPathIndex - mCurrentPathIndex < CountBetweenCurAndLast)
                GeneratePath(PathCellPrefabs[randomIndex]);
        }
    }

    private void GeneratePath(GameObject path)
    {
        int start_random = Random.Range(0, path.GetComponent<PathCell>().PathStartJointList.Count);
        int end_random = Random.Range(0, mCurrentLastPathObj.GetComponent<PathCell>().PathEndJointList.Count);

        Vector3 Offset = path.transform.position - path.GetComponent<PathCell>().PathStartJointList[start_random].position;
        GameObject pathObj = Instantiate(path, mCurrentLastPathObj.GetComponent<PathCell>().PathEndJointList[end_random].position + Offset, Quaternion.identity, PathRoot);
        mCurrentLastPathObj = pathObj;
        mCurrentLastPathIndex++;
        PathDic.Add(mCurrentLastPathObj, mCurrentLastPathIndex);
    }

    //为错误的方向生成路径
    private void GeneratePathForWrongDirection()
    {
        
    }
}
