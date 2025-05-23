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

public struct PlayerSteeringEvent
{
  public float Steering;
}

public struct PlayerInRangeEvent { }
public struct PlayerOutOfRangeEvent { }
public struct DisableThirdPersonControlsEvent {}