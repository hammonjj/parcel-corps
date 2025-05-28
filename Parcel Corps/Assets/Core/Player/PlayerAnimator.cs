using UnityEngine;

public class PlayerAnimator : MonoBehaviourBase
{
  private MessageBus _messageBus;
  [SerializeField] private Animator _animator;

  protected override void Awake()
  {
    base.Awake();
  }

  protected override void OnEnable()
  {
    base.OnEnable();

    _messageBus = GameObject.FindWithTag("PlayerMessageBus")?.GetComponent<MessageBus>();
    _messageBus?.Subscribe<PlayerMovementEvent>(OnPlayerMovementEvent);
    _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnPlayerExitVehicle);
    _sceneMessageBus.Subscribe<PlayerVehicleSeatEvent>(OnPlayerVehicleSeatEvent);
  }

  private void OnPlayerExitVehicle(PlayerExitVehicleEvent @event)
  {
    _animator.SetBool("isDriving", false);
    _animator.SetBool("isPassenger", false);
  }

  private void OnPlayerVehicleSeatEvent(PlayerVehicleSeatEvent @event)
  {
    LogDebug($"OnPlayerVehicleSeatEvent: Row: {@event.Row}, " +
             $"isPassenger: {@event.isPassenger}, isDriver: {@event.isDriver}");

    _animator.SetBool("isRunning", false);
    _animator.SetBool("isDriving", @event.isDriver);
    _animator.SetBool("isPassenger", !@event.isDriver);
  }

  //⚠️ This method must be public, take no parameters, and exist on the same GameObject as the Animator.
  //   public void OnEnterVehicleFinished()
  // {
  //     LogDebug("EnterVehicle animation completed.");
  //     _animator.SetBool("isDriving", true);
  // }

  private void OnPlayerMovementEvent(PlayerMovementEvent @event)
  {
    var isRunning = @event.HorizontalMovement != 0 || @event.VerticalMovement != 0;
    if(_animator.GetBool("isRunning") == isRunning)
    {
      return;
    }

    LogDebug("Movement change received");

    _animator.SetBool("isRunning", isRunning);
  }
}