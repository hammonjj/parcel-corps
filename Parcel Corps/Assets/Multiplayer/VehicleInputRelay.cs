using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleInputRelay : MonoBehaviourBase
{
    public InputActionReference throttleAction;
    public InputActionReference steerAction;
    public InputActionReference brakeAction;

    public NetworkIdentity Identity;
    public CommandQueue CommandQueue;

    protected override void Awake()
    {
        base.Awake();
        if (CommandQueue == null)
        {
            CommandQueue = FindFirstObjectByType<GameLoop>()?.CommandQueue;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (throttleAction != null) {
            throttleAction.action.Enable();
        }

        if (steerAction != null) {
            steerAction.action.Enable();
        }

        if (brakeAction != null) {
            brakeAction.action.Enable();
        }
    }

    protected void OnDisable()
    {
        if (throttleAction != null) {
            throttleAction.action.Disable();
        }

        if (steerAction != null) {
            steerAction.action.Disable();
        }

        if (brakeAction != null) {
            brakeAction.action.Disable();
        }
    }

    private void Update()
    {
        if (Identity == null || !Identity.IsLocal)
        {
            return;
        }

        float throttle = throttleAction?.action.ReadValue<float>() ?? 0f;
        float steer = steerAction?.action.ReadValue<float>() ?? 0f;
        float brake = brakeAction?.action.ReadValue<float>() ?? 0f;

        if (throttle != 0f || steer != 0f || brake != 0f)
        {
            Debug.Log($"Throttle: {throttle}, Steer: {steer}, Brake: {brake}");
            return;
        }
        
        CommandQueue.Enqueue(Identity.Id, new SetThrottleCommand { Throttle = throttle });
        CommandQueue.Enqueue(Identity.Id, new SetSteeringCommand { Steering = steer });
        CommandQueue.Enqueue(Identity.Id, new SetBrakeCommand { BrakeAmount = brake });
    }
}
