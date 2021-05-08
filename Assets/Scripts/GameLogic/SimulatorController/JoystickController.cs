using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehiclePhysics;

public class JoystickController : SimulatorControllerBase
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
        base.Update();

        if (this.Logic.CurrentState != this.State.Stop)
        {
            if (!JoystickManager.Instance.dataFromSimulator.SimulatorInput.Power)
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
        if (JoystickManager.Instance.dataFromSimulator.SimulatorInput.Power)
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
        if (JoystickManager.Instance.dataFromSimulator.SimulatorInput.Fire)
        {
            StartPressed();
        }
        else
        {
            ReleaseKey();
        }

        GetInputs();
    }

    private void GetInputs()
    {
        vpStandardInput.externalHandbrake = JoystickManager.Instance.dataFromSimulator.SimulatorInput.HandBrake ? 1 : 0;
        vpStandardInput.externalClutch = JoystickManager.Instance.dataFromSimulator.SimulatorInput.ClutchAxis;
        vpStandardInput.externalThrottle = JoystickManager.Instance.dataFromSimulator.SimulatorInput.ThrottleAxis;
        vpStandardInput.externalBrake = JoystickManager.Instance.dataFromSimulator.SimulatorInput.BrakeAxis;
        vpStandardInput.externalSteer = JoystickManager.Instance.dataFromSimulator.SimulatorInput.SteerAxis;
        GearInput();
    }

    /// <summary>
    /// 档位
    /// </summary>
    private void GearInput()
    {
        switch (JoystickManager.Instance.dataFromSimulator.SimulatorInput.Gear)
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
                SimulateKeyboard.KeyDown(KeyCode.N);
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
