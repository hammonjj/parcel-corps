public class SetThrottleCommand : IPlayerCommand
{
    public float Throttle;

    public void Execute(GameState state, string senderId)
    {
        var player = state.GetPlayer(senderId);
        if (player is VehiclePlayerEntity vehicle)
        {
            vehicle.ApplyThrottle(Throttle);
        }
    }
}
