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
    _sceneMessageBus.Subscribe<PlayerEnterVehicleEvent>(OnPlayerEnterVehicle);
    _sceneMessageBus.Subscribe<PlayerExitVehicleEvent>(OnPlayerExitVehicle);
  }

  private void OnPlayerExitVehicle(PlayerExitVehicleEvent @event)
  {
    _animator.SetBool("isDriving", false);
  }

  private void OnPlayerEnterVehicle(PlayerEnterVehicleEvent @event)
  {
    _animator.SetBool("isRunning", false);
    _animator.SetBool("isDriving", true);
  }

  protected void OnDisable()
  {
    _messageBus?.Unsubscribe<PlayerMovementEvent>(OnPlayerMovementEvent);
  }

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