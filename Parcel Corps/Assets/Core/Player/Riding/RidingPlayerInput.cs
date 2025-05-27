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

        _sceneMessageBus.Subscribe<PlayerEnterVehicleEvent>(OnEnterVehicle);
        _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnExitVehicle);

        DisableActions();
    }

    private void OnEnterVehicle(PlayerEnterVehicleEvent evt)
    {
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
