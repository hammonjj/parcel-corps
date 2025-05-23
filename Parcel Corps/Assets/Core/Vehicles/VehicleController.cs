using UnityEngine;

public class VehicleController : MonoBehaviourBase
{    
  [SerializeField] private VehicleState _vehicleState;
  [SerializeField] private MessageBus _messageBus;
  [SerializeField] private Transform _driverSeat;

  protected override void OnEnable()
  {
    base.OnEnable();
    if (_messageBus == null)
    {
      LogError("MessageBus not assigned");
    }

    _sceneMessageBus.Subscribe<PlayerEnterVehicleEvent>(OnPlayerEnterVehicle);
    _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnPlayerExitVehicle);
  }

  private void OnPlayerExitVehicle(PlayerExitVehicleEvent @event)
  {
    
  }

  private void OnPlayerEnterVehicle(PlayerEnterVehicleEvent @event)
  {
    LogDebug("Player entered vehicle.");
    var player = GameObject.FindWithTag("Player");
    if (player != null)
    {
      player.transform.position = _driverSeat.position;
      player.transform.rotation = _driverSeat.rotation;
    }
    else
    {
      LogError("Player not found");
    }
  }
}
