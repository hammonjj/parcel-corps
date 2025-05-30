using UnityEngine;

public class VehicleAnimator : MonoBehaviourBase
{
    [SerializeField] private Transform _frontLeftWheel;
    [SerializeField] private Transform _frontRightWheel;
    [SerializeField] private Transform _rearLeftWheel;
    [SerializeField] private Transform _rearRightWheel;

    private MessageBus _messageBus;

    protected override void OnEnable()
    {
        base.OnEnable();

        _messageBus = GetComponent<MessageBus>();
        _messageBus.Subscribe<VehicleSteerEvent>(OnVehicleSteer);
    }

  private void OnVehicleSteer(VehicleSteerEvent @event)
  {
    _frontLeftWheel.localRotation = Quaternion.Euler(0, @event.SteeringAngle, 0);
    _frontRightWheel.localRotation = Quaternion.Euler(0, @event.SteeringAngle, 0);
  }
}
