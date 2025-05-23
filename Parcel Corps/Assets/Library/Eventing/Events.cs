public struct DisableInputExceptActionEvent { }

public struct EnableInputExceptActionEvent { }

public struct PlayerActionButtonEvent { }

public struct PlayerMovementEvent
{
  public float HorizontalMovement;
  public float VerticalMovement;
}

public struct PlayerEnterVehicleEvent { }

public struct PlayerExitVehicleEvent { }

public struct PlayerThrottleEvent
{
  public float Throttle;
}

public struct PlayerBrakeEvent
{
  public float Brake;
}

public struct PlayerSteeringEvent
{
  public float Steering;
}

public struct PlayerInRangeEvent { }
public struct PlayerOutOfRangeEvent { }
public struct DisableThirdPersonControlsEvent {}