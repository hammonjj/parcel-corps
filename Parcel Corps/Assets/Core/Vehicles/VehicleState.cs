using System;
using UnityEngine;

public class VehicleState : MonoBehaviourBase
{
    public bool isPlayerInRange = false;
    public bool isPlayerInVehicle = false;
    
    [SerializeField] private MessageBus _messageBus;

  protected override void OnEnable()
  {
    base.OnEnable();
    if (_messageBus == null)
    {
      LogError("MessageBus not assigned");
    }

    _messageBus.Subscribe<PlayerInRangeEvent>(OnPlayerInRange);
    _messageBus.Subscribe<PlayerOutOfRangeEvent>(OnPlayerOutOfRange);
    _sceneMessageBus.Subscribe<PlayerActionButtonEvent>(OnPlayerActionButton);
  }

  private void OnPlayerActionButton(PlayerActionButtonEvent @event)
  {
    if (!isPlayerInRange)
    {
      LogDebug("Player is not in range.");
      return;
    }

    if (!isPlayerInVehicle)
    {
      LogDebug("Player is entering vehicle.");
      isPlayerInVehicle = true;
      _sceneMessageBus.Publish(new PlayerEnterVehicleEvent { VehicleMessageBus = _messageBus });
    }
  }

  private void OnPlayerOutOfRange(PlayerOutOfRangeEvent @event)
  {
    isPlayerInRange = false;
    LogDebug("Player out of range.");
  }

  private void OnPlayerInRange(PlayerInRangeEvent @event)
  {
    isPlayerInRange = true;
    LogDebug("Player in range.");
  }
}
