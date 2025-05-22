using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonPlayerInput : MonoBehaviourBase
{
    [SerializeField] public InputActionReference actionAction;
    [SerializeField] public InputActionReference verticalAction;
    [SerializeField] public InputActionReference horizontalAction;

    private IMessageBus _messageBus;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _messageBus = GameObject.FindWithTag("PlayerMessageBus")?.GetComponent<MessageBus>();

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

    protected void OnDisable()
    {
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

    private void Update()
    {
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
        Debug.Log("Action performed");
        _messageBus?.Publish(new PlayerActionButtonEvent { });
    }
}
