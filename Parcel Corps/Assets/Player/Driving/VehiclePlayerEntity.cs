using UnityEngine;

public class VehiclePlayerEntity : PlayerEntity
{
    private float _throttleInput;
    private float _steeringInput;
    private float _brakeInput;

    private float _speed = 0f;
    private float _angle = 0f;

    private readonly float _maxSpeed;
    private readonly float _acceleration;
    private readonly float _brakeForce;
    private readonly float _turnRate;

    public VehiclePlayerEntity(string id, Vector2 startPosition, VehicleConfig config) : base(id, startPosition)
    {
        _maxSpeed = config.maxSpeed;
        _acceleration = config.acceleration;
        _brakeForce = config.brakeForce;
        _turnRate = config.turnRate;
    }

    public void ApplyThrottle(float value)
    {
        _throttleInput = Mathf.Clamp(value, -1f, 1f);
    }

    public void ApplySteering(float value)
    {
        _steeringInput = Mathf.Clamp(value, -1f, 1f);
    }

    public void ApplyBrake(float value)
    {
        _brakeInput = Mathf.Clamp01(value);
    }

    public override void Tick(float deltaTime)
    {
        if (_brakeInput > 0f)
        {
            _speed = Mathf.MoveTowards(_speed, 0f, _brakeForce * _brakeInput * deltaTime);
        }
        else
        {
            _speed += _throttleInput * _acceleration * deltaTime;
        }

        _speed = Mathf.Clamp(_speed, -_maxSpeed, _maxSpeed);

        _angle += _steeringInput * _turnRate * deltaTime;

        Vector2 direction = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
        Position += direction.normalized * _speed * deltaTime;
    }
}
