using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using fsm;

/// <summary>
/// 反射工具类，用于测试信息
/// </summary>
public class ReflectionTools
{

    /// <summary>
    /// 状态机获取当前状态名称
    /// </summary>
    /// <typeparam name="T">状态类</typeparam>
    /// <param name="stateMachine">当前脚本状态机</param>
    /// <param name="state">状态实例</param>
    /// <param name="propertyName">CurrentState</param>
    /// <returns>当前状态名称</returns>
    public static string GetCurrentStateName<T>(StateMachine stateMachine, T state, string propertyName)
    {
        PropertyInfo _pinfo = typeof(StateMachine).GetProperty(propertyName);
        FieldInfo[] _finfos = typeof(T).GetFields();

        foreach (FieldInfo i in _finfos)
        {
            if (_pinfo.GetValue(stateMachine) == i.GetValue(state))
                return i.Name;
        }

        return null;
    }
}
