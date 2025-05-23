using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingPlayerInput : MonoBehaviourBase
{
    private bool _actionsDisabled = false;
    [SerializeField] public InputActionReference actionAction;
    [SerializeField] public InputActionReference steeringAction;
    [SerializeField] public InputActionReference acceleratorAction;
    [SerializeField] public InputActionReference brakeAction;

    private MessageBus _messageBus;
    private MessageBus? _vehicleMessageBus;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _messageBus = GameObject.FindWithTag("PlayerMessageBus")?.GetComponent<MessageBus>();
        _sceneMessageBus.Subscribe<PlayerEnterVehicleEvent>(OnPlayerEnterVehicle);
        _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnPlayerExitVehicle);

        DisableActions();
    }

    private void OnPlayerExitVehicle(PlayerExitVehicleEvent @event)
    {
        DisableActions();
        _vehicleMessageBus = null;
    }

    private void OnPlayerEnterVehicle(PlayerEnterVehicleEvent @event)
    {
        EnableActions();
        _vehicleMessageBus = @event.VehicleMessageBus;
    }

    protected void OnDisable()
    {
        DisableActions();
    }

    private void Update()
    {
        if (_actionsDisabled)
        {
            return;
        }

        var playerDrivingEvent = new PlayerDrivingEvent
        {
            Steering = steeringAction?.action.ReadValue<float>() ?? 0f,
            Throttle = acceleratorAction?.action.ReadValue<float>() ?? 0f,
            Brake = brakeAction?.action.ReadValue<float>() ?? 0f
        };

        _vehicleMessageBus?.Publish(playerDrivingEvent);
    }

    private void OnActionAction(InputAction.CallbackContext context)
    {
        Debug.Log("Action performed");
        _messageBus?.Publish(new PlayerActionButtonEvent { });
    }
    
    private void DisableActions()
    {
        _actionsDisabled = true;

        if (steeringAction != null)
        {
            steeringAction.action.Disable();
        }

        if (acceleratorAction != null)
        {
            acceleratorAction.action.Disable();
        }

        if (actionAction != null)
        {
            actionAction.action.Disable();
        }
    }
    private void EnableActions()
    {
        _actionsDisabled = false;

        if (steeringAction != null)
        {
            steeringAction.action.Enable();
        }

        if (acceleratorAction != null)
        {
            acceleratorAction.action.Enable();
        }

        if (actionAction != null)
        {
            actionAction.action.Enable();
            actionAction.action.performed += OnActionAction;
        }
        if (brakeAction != null)
        {
            brakeAction.action.Enable();
        }
    }
}
