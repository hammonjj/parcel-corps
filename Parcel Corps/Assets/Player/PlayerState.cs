using UnityEngine;

public class PlayerState : MonoBehaviourBase
{
    public enum InputState
    {
        ThirdPerson,
        InVehicle
    }

    private IMessageBus _messageBus;
    [SerializeField] private InputState currentState = InputState.ThirdPerson;

    protected override void Awake()
    {
        base.Awake();
        
        _messageBus = GameObject.FindWithTag("PlayerMessageBus")?.GetComponent<MessageBus>();
    }
}
