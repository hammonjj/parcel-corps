using UnityEngine;

public class InteractionTrigger : MonoBehaviourBase
{
    [SerializeField] private MessageBus _messageBus;

    protected override void Awake()
    {
        base.Awake();
        if (_messageBus == null)
        {
            LogError("MessageBus not assigned");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }
        
        LogDebug("Player entered the trigger zone.");
        _messageBus?.Publish(new PlayerInRangeEvent());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) { return; }

        LogDebug("Player exited the trigger zone.");
        _messageBus?.Publish(new PlayerOutOfRangeEvent());
    }
}
