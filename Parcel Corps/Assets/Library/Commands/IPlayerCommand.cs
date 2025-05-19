public interface IPlayerCommand
{
    void Execute(GameState state, string senderId);
}
