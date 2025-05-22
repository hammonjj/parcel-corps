using UnityEngine;

public class ThirdPersonPlayerAnimator : MonoBehaviourBase
{
  private IMessageBus _messageBus;
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
    _messageBus?.Subscribe<PlayerActionButtonEvent>(OnPlayerActionEvent);
  }

  protected void OnDisable()
  {
    _messageBus?.Unsubscribe<PlayerMovementEvent>(OnPlayerMovementEvent);
    _messageBus?.Unsubscribe<PlayerActionButtonEvent>(OnPlayerActionEvent);
  }

  private void OnPlayerActionEvent(PlayerActionButtonEvent @event)
  {
    LogDebug("Action received");
    _animator.SetTrigger("sit");
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