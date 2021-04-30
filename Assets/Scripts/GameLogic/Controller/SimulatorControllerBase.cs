using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using fsm;

public class SimulatorControllerBase : MonoBehaviour
{
    public class States
    {
        public IState Stop;
        public IState PowerOn;
    }
    public StateMachine Logic = new StateMachine();
    public States State = new States();

    /// <summary>
    /// 判断是否忽略输入
    /// </summary>
    public bool IgnoreInput = false;

    protected void Awake()
    {
        this.State.Stop = new State
        {
            OnEnterEvent = new Action(this.OnEnterStopState),
            UpdateStateEvent = new Action(this.UpdateStopState)
        };
        this.State.PowerOn = new State
        {
            OnEnterEvent = new Action(this.OnEnterPowerOnState),
            UpdateStateEvent = new Action(this.UpdatePowerOnState)
        };
        this.Logic.RegisterStates(new IState[]
        {
            this.State.Stop,
            this.State.PowerOn,
        });
        this.Logic.ChangeState(this.State.Stop);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //忽略输入响应
        if (IgnoreInput)
            return;

        this.Logic.UpdateState(Time.deltaTime);
    }

    protected virtual void OnEnterStopState()
    {

    }

    protected virtual void UpdateStopState()
    {

    }

    protected virtual void OnEnterPowerOnState()
    {

    }

    protected virtual void UpdatePowerOnState()
    {

    }
}
