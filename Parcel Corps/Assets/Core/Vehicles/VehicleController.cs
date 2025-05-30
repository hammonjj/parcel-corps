using UnityEngine;

public class VehicleController : MonoBehaviourBase
{
  [Header("Vehicle Transforms")]
  [SerializeField] private VehicleState _vehicleState;
  [SerializeField] private MessageBus _messageBus;
  [SerializeField] private Transform _driverSeat;
  [SerializeField] private Transform _driverExit;

  [SerializeField] private Transform[] _loadingPoints;
  [SerializeField] private Transform[] _ridingPoints;

  [Header("Wheels")]
  [SerializeField] private WheelCollider _frontLeftWheel;
  [SerializeField] private WheelCollider _frontRightWheel;
  [SerializeField] private WheelCollider _rearLeftWheel;
  [SerializeField] private WheelCollider _rearRightWheel;

  [SerializeField] private Transform _frontLeftTransform;
  [SerializeField] private Transform _frontRightTransform;
  [SerializeField] private Transform _rearLeftTransform;
  [SerializeField] private Transform _rearRightTransform;

  //Convert to scriptable object later
  [Header("Vehicle Settings")]
  [SerializeField] private BaseVehicleData _vehicleData;

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

    // 3) Snap them into the seat -> Might want to move this to the SeatController and move the driving piece there as well
    var closestIndex = GameObjectUtils.FindClosestTransformIndex(_playerRoot, _loadingPoints);
    
    _playerRoot.position = _ridingPoints[closestIndex].position;
    _playerRoot.rotation = _ridingPoints[closestIndex].rotation;

    // 4) Parent so they ride along
    _playerRoot.SetParent(_ridingPoints[closestIndex], worldPositionStays: true);

    _ridingPoints[closestIndex].gameObject.GetComponent<SeatController>()?.SetPlayerInSeat(true);
  }

  private void ApplySteering()
  {
    var steer = _steering * _vehicleData.maxSteeringAngle;
    _frontLeftWheel.steerAngle = steer;
    _frontRightWheel.steerAngle = steer;
  }

  private void ApplyTorque()
  {
    float motorTorque = _throttle * _vehicleData.maxMotorTorque;
    _frontLeftWheel.motorTorque = motorTorque;
    _frontRightWheel.motorTorque = motorTorque;
  }

  private void ApplyBrake()
  {
    if (_brake > 0f)
    {
      _frontLeftWheel.brakeTorque = _brake * _vehicleData.brakeTorque;
      _frontRightWheel.brakeTorque = _brake * _vehicleData.brakeTorque;
      _rearLeftWheel.brakeTorque = _brake * _vehicleData.brakeTorque;
      _rearRightWheel.brakeTorque = _brake * _vehicleData.brakeTorque;
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
    ApplyBrake();
  }
}
