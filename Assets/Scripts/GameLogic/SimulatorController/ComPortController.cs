using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class ComPortController : SimulatorControllerBase
{
    //测试车辆本体
    public VehicleBase vehicle;

    /// <summary>
    /// VP插件输入管理
    /// </summary>
    public VPStandardInput vpStandardInput;

    // Update is called once per frame
    protected override void Update()
    {
        //Debug.Log(ReflectionTools.GetCurrentStateName(this.Logic, this.State, "CurrentState"));

        base.Update();

        if (this.Logic.CurrentState != this.State.Stop)
        {
            if (!ComPortManager.Instance.dataFromSimulator.ComInput.Power)
                this.Logic.ChangeState(this.State.Stop);
        }
    }

    protected override void OnEnterStopState()
    {
        OffPressed();
    }

    protected override void UpdateStopState()
    {
        //电源
        if (ComPortManager.Instance.dataFromSimulator.ComInput.Power)
        {
            AccOnPressed();
            this.Logic.ChangeState(this.State.PowerOn);
        }
    }

    protected override void OnEnterPowerOnState()
    {

    }

    protected override void UpdatePowerOnState()
    {
        if (ComPortManager.Instance.dataFromSimulator.ComInput.Fire)
        {
            StartPressed();
        }
        else
        {
            ReleaseKey();
        }

        HandBrakeInput();
        BrakeInput();
        ClutchInput();
        AcceleratorInput();
        DirectionInput();
        GearInput();
    }

    /// <summary>
    /// 手刹
    /// </summary>
    private void HandBrakeInput()
    {
        vpStandardInput.externalHandbrake = ComPortManager.Instance.dataFromSimulator.ComInput.HandBrake ? 0 : 1;
    }

    /// <summary>
    /// 离合(0 ~ 1)
    /// </summary>
    private void ClutchInput()
    {
        vpStandardInput.externalClutch = ComPortManager.Instance.dataFromSimulator.ComInput.Clutch;
    }

    /// <summary>
    /// 油门Throttle(-88 ~ 89)
    /// </summary>
    private void AcceleratorInput()
    {
        vpStandardInput.externalThrottle = (ComPortManager.Instance.dataFromSimulator.ComInput.Accelerator + 88f) / 177f;
    }

    /// <summary>
    /// 刹车(-0.0117 ~ -0.9764)
    /// </summary>
    private void BrakeInput()
    {
        vpStandardInput.externalBrake = -ComPortManager.Instance.dataFromSimulator.ComInput.Brake;
    }

    /// <summary>
    /// 方向盘(-0.85 ~ 0.89)
    /// </summary>
    private void DirectionInput()
    {
        vpStandardInput.externalSteer = -(ComPortManager.Instance.dataFromSimulator.ComInput.SteeringWheel - 0.02f) / 0.87f;
    }

    /// <summary>
    /// 档位
    /// </summary>
    private void GearInput()
    {
        switch(ComPortManager.Instance.dataFromSimulator.ComInput.Gear)
        {
            case 0:
                SimulateKeyboard.KeyDown(KeyCode.N);
                break;
            case 1:
                SimulateKeyboard.KeyDown(KeyCode.Alpha1);
                break;
            case 2:
                SimulateKeyboard.KeyDown(KeyCode.Alpha2);
                break;
            case 3:
                SimulateKeyboard.KeyDown(KeyCode.Alpha3);
                break;
            case 4:
                SimulateKeyboard.KeyDown(KeyCode.Alpha4);
                break;
            case 5:
                SimulateKeyboard.KeyDown(KeyCode.Alpha5);
                break;
            case 6:
                SimulateKeyboard.KeyDown(KeyCode.R);
                break;
            default:
                break;
        }
    }

    private void StartPressed()
    {
        if (vehicle == null) return;

        // If key was Off, move to Acc-On.
        // If it was Acc-On, move to Start.

        int key = vehicle.data.Get(Channel.Input, InputData.Key);

        if (key == -1)
            vehicle.data.Set(Channel.Input, InputData.Key, 0);
        else
        if (key == 0)
            vehicle.data.Set(Channel.Input, InputData.Key, 1);
    }

    private void AccOnPressed()
    {
        if (vehicle == null) return;

        // Move key to Acc-On

        vehicle.data.Set(Channel.Input, InputData.Key, 0);
    }

    private void OffPressed()
    {
        if (vehicle == null) return;

        // Move key to Off

        vehicle.data.Set(Channel.Input, InputData.Key, -1);
    }

    private void ReleaseKey()
    {
        if (vehicle == null) return;

        // If Start was pressed, move back to Acc-On.

        int key = vehicle.data.Get(Channel.Input, InputData.Key);

        if (key == 1)
            vehicle.data.Set(Channel.Input, InputData.Key, 0);
    }
}
