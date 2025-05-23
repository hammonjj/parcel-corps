public struct PlayerActionButtonEvent { }

public struct PlayerMovementEvent
{
  public float HorizontalMovement;
  public float VerticalMovement;
}

public struct PlayerEnterVehicleEvent
{ 
  public MessageBus VehicleMessageBus;
}

public struct PlayerExitVehicleEvent { }

public struct PlayerDrivingEvent
{
  public float Throttle;
  public float Brake;
  public float Steering;
}

public struct PlayerInRangeEvent { }
public struct PlayerOutOfRangeEvent { }
public struct DisableThirdPersonControlsEvent {}