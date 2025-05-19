public class SetBrakeCommand : IPlayerCommand
{
    public float BrakeAmount;

    public void Execute(GameState state, string senderId)
    {
        var player = state.GetPlayer(senderId);
        if (player is VehiclePlayerEntity vehicle)
        {
            vehicle.ApplyBrake(BrakeAmount);
        }
    }
}
