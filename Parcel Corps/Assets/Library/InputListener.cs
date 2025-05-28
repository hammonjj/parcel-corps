using UnityEngine;
using UnityEngine.InputSystem;

public class InputListener : MonoBehaviourBase
{
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private string thirdPersonMap = "ThirdPerson";
    [SerializeField] private string drivingMap = "Driving";
    [SerializeField] private string ridingMap = "Riding";

    protected override void OnEnable()
    {
        base.OnEnable();
        LogDebug("InputListener enabled.");
        _sceneMessageBus.Subscribe<PlayerEnterVehicleEvent>(OnEnterVehicle);
        _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnExitVehicle);
        _sceneMessageBus.Subscribe<PlayerVehicleSeatEvent>(OnVehicleSeat);
    }

    protected void OnDisable()
    {
        _sceneMessageBus.Unsubscribe<PlayerEnterVehicleEvent>(OnEnterVehicle);
        _sceneMessageBus.Unsubscribe<PlayerExitVehicleEvent>(OnExitVehicle);
        _sceneMessageBus.Unsubscribe<PlayerVehicleSeatEvent>(OnVehicleSeat);
    }

    private void Start()
    {
        LogDebug("InputListener started.");
        SwitchToMap(thirdPersonMap);
    }

    private void OnVehicleSeat(PlayerVehicleSeatEvent @event)
    {
        SwitchToMap(@event.isDriver ? drivingMap : ridingMap);
        LogDebug($"Switched to Riding input map for row {@event.Row}, " +
            $"isPassenger: {@event.isPassenger}, isDriver: {@event.isDriver}");
    }

    private void OnEnterVehicle(PlayerEnterVehicleEvent _)
    {
        //SwitchToMap(drivingMap);
        //LogDebug("Switched to Driving input map.");
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
