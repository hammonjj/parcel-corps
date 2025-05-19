using UnityEngine;

public class MoveCommand : IPlayerCommand
{
    public Vector2 Direction;

    public void Execute(GameState state, string senderId)
    {
        var player = state.GetPlayer(senderId);

        if (player != null)
        {
            player.TryMove(Direction, Time.deltaTime);
        }
    }
}
