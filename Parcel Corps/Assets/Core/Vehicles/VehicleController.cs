using System;
using UnityEngine;

public class VehicleController : MonoBehaviourBase
{    
  [SerializeField] private VehicleState _vehicleState;
  [SerializeField] private MessageBus _messageBus;
  [SerializeField] private Transform _driverSeat;

  [SerializeField] private WheelCollider _frontLeftWheel;
  [SerializeField] private WheelCollider _frontRightWheel;
  [SerializeField] private WheelCollider _rearLeftWheel;
  [SerializeField] private WheelCollider _rearRightWheel;

  [SerializeField] private Transform _frontLeftTransform;
  [SerializeField] private Transform _frontRightTransform;
  [SerializeField] private Transform _rearLeftTransform;
  [SerializeField] private Transform _rearRightTransform;

  //Convert to scriptable object later
  [SerializeField] private float _maxMotorTorque = 1500f;
  [SerializeField] private float _maxSteeringAngle = 30f;
  [SerializeField] private float _brakeTorque = 3000f;
  [SerializeField] private float _maxSpeed = 100f;
  [SerializeField] private float _acceleration = 10f;

  protected override void OnEnable()
  {
    base.OnEnable();
    if (_messageBus == null)
    {
      LogError("MessageBus not assigned");
    }

    _sceneMessageBus.Subscribe<PlayerEnterVehicleEvent>(OnPlayerEnterVehicle);
    _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnPlayerExitVehicle);
    _messageBus.Subscribe<PlayerDrivingEvent>(OnPlayerDriving);
  }

  private void OnPlayerDriving(PlayerDrivingEvent @event)
  {
    LogDebug("OnPlayerDriving");
    
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
