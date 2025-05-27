using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingPlayerInput : MonoBehaviourBase
{
    private bool _actionsDisabled = false;
    [SerializeField] public InputActionReference actionAction;
    [SerializeField] public InputActionReference steeringAction;
    [SerializeField] public InputActionReference acceleratorAction;
    [SerializeField] public InputActionReference brakeAction;

    private ResettableBool _inputDebounce = new();
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
        _inputDebounce.SetTrue();
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
        Debug.Log("Driving Action Performed");

        if (_inputDebounce.TryConsume())
        {
            LogDebug("Input debounce active, ignoring driving action event.");
            return;
        }

        _sceneMessageBus?.Publish(new PlayerDrivingActionButtonEvent { });
    }
    
    private void DisableActions()
    {
        LogDebug("Disabling Driving Actions");
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
        LogDebug("Enabling Driving Actions");
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
