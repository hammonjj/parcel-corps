
using System;
using UnityEngine;

public class ThirdPersonPlayerMovement : MonoBehaviourBase
{
    [SerializeField] private float _speed = 5f;

    private Vector3 _movement;
    private Rigidbody _rb;
    private MessageBus _messageBus;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _messageBus = GameObject.FindWithTag("PlayerMessageBus")?.GetComponent<MessageBus>();
        _sceneMessageBus.Subscribe<PlayerEnterVehicleEvent>(OnPlayerEnterVehicle);
        _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnPlayerExitVehicle);
        EnableSubscriptions();

        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }
    }

  private void OnPlayerExitVehicle(PlayerExitVehicleEvent @event)
  {
    EnableSubscriptions();
  }

  private void OnPlayerEnterVehicle(PlayerEnterVehicleEvent @event)
  {
    DisableSubscriptions();
  }

  private void OnDisable()
    {
        DisableSubscriptions();
    }

    private void EnableSubscriptions()
    {
        LogDebug("Enabling player movement subscriptions");
        _messageBus?.Subscribe<PlayerMovementEvent>(OnPlayerMovementEvent);
        _messageBus?.Subscribe<PlayerActionButtonEvent>(OnPlayerActionEvent);
    }
    private void DisableSubscriptions()
    {
        LogDebug("Disabling player movement subscriptions");
        _messageBus?.Unsubscribe<PlayerMovementEvent>(OnPlayerMovementEvent);
        _messageBus?.Unsubscribe<PlayerActionButtonEvent>(OnPlayerActionEvent);
    }

    private void OnPlayerActionEvent(PlayerActionButtonEvent @event)
    {
        LogDebug("Action received");
    }

    private void OnPlayerMovementEvent(PlayerMovementEvent @event)
    {
        _movement = new Vector3(@event.HorizontalMovement, 0, @event.VerticalMovement);
    }

    private void Move(Vector3 movement)
    {
        var matrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        var skewedMovement = matrix.MultiplyPoint3x4(movement);

        Vector3 horizontalVel = skewedMovement * _speed;
        Vector3 newVel = new Vector3(
            horizontalVel.x,
            _rb.linearVelocity.y,        // preserve whatever Unity’s gravity has done
            horizontalVel.z
        );

        _rb.linearVelocity = newVel;
        //_rb.MovePosition(_rb.position + skewedMovement * _speed * Time.deltaTime);

        // Face movement direction
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(skewedMovement);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    private void FixedUpdate()
    {
        Move(_movement);
    }
}
