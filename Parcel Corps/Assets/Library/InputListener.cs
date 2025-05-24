using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviourBase
{
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private string thirdPersonMap = "ThirdPerson";
    [SerializeField] private string drivingMap = "Driving";

    protected override void OnEnable()
    {
        base.OnEnable();

        SwitchToMap(thirdPersonMap);
        _sceneMessageBus.Subscribe<PlayerEnterVehicleEvent>(OnEnterVehicle);
        _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnExitVehicle);
    }

    protected void OnDisable()
    {
        _sceneMessageBus.Unsubscribe<PlayerEnterVehicleEvent>(OnEnterVehicle);
        _sceneMessageBus.Unsubscribe<PlayerExitVehicleEvent>(OnExitVehicle);
    }

    private void OnEnterVehicle(PlayerEnterVehicleEvent _)
    {
        SwitchToMap(drivingMap);
        LogDebug("Switched to Driving input map.");
    }

    private void OnExitVehicle(PlayerExitVehicleEvent _)
    {
        SwitchToMap(thirdPersonMap);
        LogDebug("Switched to ThirdPerson input map.");
    }

    private void SwitchToMap(string mapName)
    {
        foreach (var map in _inputAsset.actionMaps)
        {
            if (map.name == mapName) { map.Enable(); }
            else { map.Disable(); }
        }
    }
}
