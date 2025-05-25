using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonPlayerInput : MonoBehaviourBase
{
    [SerializeField] public InputActionReference actionAction;
    [SerializeField] public InputActionReference verticalAction;
    [SerializeField] public InputActionReference horizontalAction;
    [SerializeField] private PlayerInput _playerInput;

    private bool _actionsDisabled = false;
    private MessageBus _messageBus;

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

        if (verticalAction != null)
        {
            verticalAction.action.Enable();
        }

        if (horizontalAction != null)
        {
            horizontalAction.action.Enable();
        }

        if (actionAction != null)
        {
            actionAction.action.Enable();
            actionAction.action.performed += OnActionAction;
        }
    }

    private void OnPlayerExitVehicle(PlayerExitVehicleEvent @event)
    {
        EnableActions();
    }

    private void OnPlayerEnterVehicle(PlayerEnterVehicleEvent @event)
    {
        DisableActions();
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

        var playerMovementEvent = new PlayerMovementEvent
        {
            VerticalMovement = verticalAction?.action.ReadValue<float>() ?? 0f,
            HorizontalMovement = horizontalAction?.action.ReadValue<float>() ?? 0f,
        };

        LogDebug($"Movement event - Vertical: {playerMovementEvent.VerticalMovement}, " +
            $"Horizontal: {playerMovementEvent.HorizontalMovement}");

        _messageBus?.Publish(playerMovementEvent);
    }

    private void OnActionAction(InputAction.CallbackContext context)
    {
        LogDebug("Action performed");
        _sceneMessageBus?.Publish(new PlayerActionButtonEvent { });
    }

    private void DisableActions()
    {
        LogDebug("Disabling Third Person Actions");

        _actionsDisabled = true;
        if (verticalAction != null)
        {
            verticalAction.action.Disable();
        }

        if (horizontalAction != null)
        {
            horizontalAction.action.Disable();
        }

        if (actionAction != null)
        {
            actionAction.action.Disable();
        }
    }

    private void EnableActions()
    {
        Debug.Log("Enabling Third Person Actions");

        _actionsDisabled = false;
        if (verticalAction != null)
        {
            verticalAction.action.Enable();
        }

        if (horizontalAction != null)
        {
            horizontalAction.action.Enable();
        }

        if (actionAction != null)
        {
            actionAction.action.Enable();
            actionAction.action.performed += OnActionAction;
        }
    }
}
