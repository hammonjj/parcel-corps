using UnityEngine;
using UnityEngine.InputSystem;

public class DrivingPlayerInput : MonoBehaviourBase
{
    [SerializeField] public InputActionReference actionAction;
    [SerializeField] public InputActionReference steeringAction;
    [SerializeField] public InputActionReference acceleratorAction;
    [SerializeField] public InputActionReference brakeAction;

    private IMessageBus _messageBus;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _messageBus = GameObject.FindWithTag("PlayerMessageBus")?.GetComponent<MessageBus>();

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
    }

    protected void OnDisable()
    {
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

    private void Update()
    {
        var playerSteeringEvent = new PlayerSteeringEvent
        {
            Steering = steeringAction?.action.ReadValue<float>() ?? 0f
        };

        LogDebug($"Steering event: {playerSteeringEvent.Steering}");

        _messageBus?.Publish(playerSteeringEvent);
    }

    private void OnActionAction(InputAction.CallbackContext context)
    {
        Debug.Log("Action performed");
        _messageBus?.Publish(new PlayerActionButtonEvent { });
    }
}
