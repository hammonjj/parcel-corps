using System;
using UnityEngine;

public class VehicleController : MonoBehaviourBase
{
  [SerializeField] private VehicleState _vehicleState;
  [SerializeField] private MessageBus _messageBus;
  [SerializeField] private Transform _driverSeat;
  [SerializeField] private Transform _driverExit;

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

  private Rigidbody _playerRb;
  private Collider _playerCol;
  private Transform _playerRoot;

  private float _steering;
  private float _throttle;
  private float _brake;

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
    _steering = @event.Steering;
    _throttle = @event.Throttle;
    _brake = @event.Brake;
  }

  private void OnPlayerExitVehicle(PlayerExitVehicleEvent @event)
  {
    LogDebug("Player exited vehicle.");
    var player = GameObject.FindWithTag("Player");
    if (player == null)
    {
      LogError("Player not found");
      return;
    }

    // 1) Unparent
    _playerRoot.SetParent(null, worldPositionStays: true);

    // 2) Move to exit marker
    _playerRoot.position = _driverExit.position;
    _playerRoot.rotation = _driverExit.rotation;

    // 3) Restore physics
    _playerRb.isKinematic = false;
    _playerRb.detectCollisions = true;
    _playerCol.enabled = true;
  }

  private void OnPlayerEnterVehicle(PlayerEnterVehicleEvent @event)
  {
    LogDebug("Player entered vehicle.");
    var player = GameObject.FindWithTag("Player");
    if (player == null)
    {
      LogError("Player not found");
      return;
    }

    // 1) Cache references
    _playerRoot = player.transform;
    _playerRb = player.GetComponent<Rigidbody>();
    _playerCol = player.GetComponent<Collider>();

    // 2) Take them out of physics
    _playerRb.isKinematic = true;
    _playerRb.detectCollisions = false;
    _playerCol.enabled = false;

    // 3) Snap them into the seat
    _playerRoot.position = _driverSeat.position;
    _playerRoot.rotation = _driverSeat.rotation;

    // 4) Parent so they ride along
    _playerRoot.SetParent(_driverSeat, worldPositionStays: true);
  }

  private void ApplySteering()
  {
    var steer = _steering * _maxSteeringAngle;
    _frontLeftWheel.steerAngle = steer;
    _frontRightWheel.steerAngle = steer;
  }

  private void ApplyTorque()
  {
    float motorTorque = _throttle * _maxMotorTorque;
    _frontLeftWheel.motorTorque = motorTorque;
    _frontRightWheel.motorTorque = motorTorque;
  }

  //I need to break the wheels out into their own gameobjects via blender before this will work
  private void ApplyBrake()
  {
    if (_brake > 0f)
    {
      _frontLeftWheel.brakeTorque = _brake * _brakeTorque;
      _frontRightWheel.brakeTorque = _brake * _brakeTorque;
      _rearLeftWheel.brakeTorque = _brake * _brakeTorque;
      _rearRightWheel.brakeTorque = _brake * _brakeTorque;
    }
    else
    {
      _frontLeftWheel.brakeTorque = 0f;
      _frontRightWheel.brakeTorque = 0f;
      _rearLeftWheel.brakeTorque = 0f;
      _rearRightWheel.brakeTorque = 0f;
    }
  }

  private void FixedUpdate()
  {
    ApplySteering();
    ApplyTorque();
    //ApplyBrake();
  }
}
