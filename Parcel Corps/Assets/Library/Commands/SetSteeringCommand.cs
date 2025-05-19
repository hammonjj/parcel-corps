public class SetSteeringCommand : IPlayerCommand
{
    public float Steering;

    public void Execute(GameState state, string senderId)
    {
        var player = state.GetPlayer(senderId);
        if (player is VehiclePlayerEntity vehicle)
        {
            vehicle.ApplySteering(Steering);
        }
    }
}
