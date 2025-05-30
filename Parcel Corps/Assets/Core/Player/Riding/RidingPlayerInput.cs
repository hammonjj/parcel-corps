using UnityEngine;
using UnityEngine.InputSystem;

public class RidingPlayerInput : MonoBehaviourBase
{
    [SerializeField] private InputActionReference aimHorizontalAction;
    [SerializeField] private InputActionReference fireAction;

    private bool _actionsDisabled = true;
    private MessageBus _vehicleMessageBus;

    protected override void OnEnable()
    {
        base.OnEnable();

        _sceneMessageBus.Subscribe<PlayerVehicleSeatEvent>(OnVehicleSeat);
        _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnExitVehicle);

        DisableActions();
    }

    private void OnVehicleSeat(PlayerVehicleSeatEvent evt)
    {
        if(evt.isDriver)
        {
            LogDebug("Not a passenger, disabling riding actions.");
            DisableActions();
            return;
        }

        _vehicleMessageBus = evt.VehicleMessageBus;
        EnableActions();
    }

    private void OnExitVehicle(PlayerExitVehicleEvent evt)
    {
        DisableActions();
        _vehicleMessageBus = null;
    }

    private void Update()
    {
        if (_actionsDisabled)
        {
            return;
        }

        float aimInput = aimHorizontalAction?.action.ReadValue<float>() ?? 0f;

        _vehicleMessageBus?.Publish(new VehicleGunAimEvent
        {
            HorizontalInput = aimInput
        });
    }

    private void OnFire(InputAction.CallbackContext ctx)
    {
        if (_actionsDisabled || ctx.performed == false)
        {
            return;
        }

        LogDebug("Firing gun from riding input.");
        _vehicleMessageBus?.Publish(new PlayerGunFireEvent());
    }

    private void EnableActions()
    {
        _actionsDisabled = false;

        aimHorizontalAction?.action.Enable();
        fireAction?.action.Enable();
        fireAction.action.performed += OnFire;
    }

    private void DisableActions()
    {
        _actionsDisabled = true;

        aimHorizontalAction?.action.Disable();
        fireAction?.action.Disable();
        fireAction.action.performed -= OnFire;
    }

    protected void OnDisable()
    {
        DisableActions();
    }
}
