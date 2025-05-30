using System;
using UnityEngine;

public class SeatController : MonoBehaviourBase
{
    [SerializeField] private Transform gunPivot;
    [SerializeField] private float aimSpeed = 90f;
    [SerializeField] private float maxArcAngle = 90f;
    [SerializeField] private float aimLineLength = 3f;
    [SerializeField] private MessageBus vehicleMessageBus;

    [SerializeField] private int _row;
    [SerializeField] private bool _isPassenger;

    private bool _isPlayerInSeat = false;
    private float _currentYaw;
    private float _targetInput;

    public void SetPlayerInSeat(bool isInSeat)
    {
        LogDebug($"SetPlayerInSeat: {isInSeat} for row {_row}, isPassenger: {_isPassenger}");
        _isPlayerInSeat = isInSeat;
        if (_isPlayerInSeat)
        {
            _currentYaw = 0f; // Reset yaw when player enters seat
        }

        if (gunPivot != null)
        {
            LogDebug($"Setting gun pivot rotation for row {_row}, isPassenger: {_isPassenger}");
            gunPivot.localRotation = Quaternion.Euler(0f, -90f, 0f);
        }

        _sceneMessageBus.Publish(new PlayerVehicleSeatEvent
        {
            Row = _row,
            isPassenger = _isPassenger,
            isDriver = _row == 0 && !_isPassenger,
            VehicleMessageBus = vehicleMessageBus
        });
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        vehicleMessageBus.Subscribe<VehicleGunAimEvent>(OnGunAim);
        vehicleMessageBus.Subscribe<PlayerVehicleSeatEvent>(OnSeatEvent);
        vehicleMessageBus.Subscribe<PlayerGunFireEvent>(OnGunFire);
    }

  private void OnGunFire(PlayerGunFireEvent @event)
  {
    //Will need to tie this in to which player is which player
    LogDebug($"OnGunFire for row {_row}, isPassenger: {_isPassenger}");
  }

  private void OnSeatEvent(PlayerVehicleSeatEvent @event)
    {
        
    }

  protected void OnDisable()
    {
        vehicleMessageBus.Unsubscribe<VehicleGunAimEvent>(OnGunAim);
    }

    private void OnGunAim(VehicleGunAimEvent evt)
    {
        _targetInput = evt.HorizontalInput;
    }

    private void Update()
    {
        if (gunPivot == null)
        {
            return;
        }

        float delta = _targetInput * aimSpeed * Time.deltaTime;
        _currentYaw = Mathf.Clamp(_currentYaw + delta, -maxArcAngle, maxArcAngle);

        //gunPivot.localRotation = Quaternion.Euler(0f, _currentYaw, 0f);
        gunPivot.localRotation = Quaternion.Euler(0f, _currentYaw - 90f, 0f);
    }

    protected override void DrawGizmosSafe()
    {
        if (showGizmos == false || gunPivot == null || (_row != 0 && _isPassenger))
        {
            return;
        }

        Vector3 origin = gunPivot.position;
        Vector3 forward = Quaternion.Euler(0f, -90f, 0f) * transform.forward;

        Gizmos.color = Color.green;
        DrawArc(origin, forward, maxArcAngle, 2f);

        Gizmos.color = Color.red;
        Vector3 aimDir = Quaternion.Euler(0f, _currentYaw - 90f, 0f) * transform.forward;
        Gizmos.DrawLine(origin, origin + aimDir * aimLineLength);
    }

    private void DrawArc(Vector3 center, Vector3 forward, float angle, float radius)
    {
        int segments = 30;
        float deltaAngle = angle * 2f / segments;
        Quaternion startRot = Quaternion.Euler(0f, -angle, 0f);
        Vector3 prevPoint = center + startRot * forward * radius;

        for (int i = 1; i <= segments; i++)
        {
            Quaternion rot = Quaternion.Euler(0f, -angle + deltaAngle * i, 0f);
            Vector3 nextPoint = center + rot * forward * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}
