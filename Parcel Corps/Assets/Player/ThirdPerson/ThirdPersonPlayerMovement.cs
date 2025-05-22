
using UnityEngine;

public class ThirdPersonPlayerMovement : MonoBehaviourBase
{
    [SerializeField] private float _speed = 5f;

    private Vector3 _movement;
    private Rigidbody _rb;
    private IMessageBus _messageBus;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _messageBus = GameObject.FindWithTag("PlayerMessageBus")?.GetComponent<MessageBus>();
        _messageBus?.Subscribe<PlayerMovementEvent>(OnPlayerMovementEvent);
        _messageBus?.Subscribe<PlayerActionButtonEvent>(OnPlayerActionEvent);

        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }
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

        _rb.MovePosition(_rb.position + skewedMovement * _speed * Time.deltaTime);

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
