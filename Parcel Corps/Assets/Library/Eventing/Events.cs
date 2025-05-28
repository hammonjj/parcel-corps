public struct PlayerActionButtonEvent { }
public struct PlayerDrivingActionButtonEvent { }

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

public struct PlayerVehicleSeatEvent
{
  public int Row;
  public bool isPassenger;
  public bool isDriver;
  public MessageBus VehicleMessageBus;
}

public struct VehicleGunAimEvent
{
  public float HorizontalInput;
}

public struct PlayerGunFireEvent { }

public struct PlayerInRangeEvent { }
public struct PlayerOutOfRangeEvent { }
public struct DisableThirdPersonControlsEvent {}