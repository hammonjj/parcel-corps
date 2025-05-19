using System.Collections.Generic;

public class CommandQueue
{
    private readonly Queue<(string senderId, IPlayerCommand command)> _queued = new();

    public void Enqueue(string senderId, IPlayerCommand command)
    {
        _queued.Enqueue((senderId, command));
    }

    public void Flush(GameState state)
    {
        while (_queued.Count > 0)
        {
            var (senderId, command) = _queued.Dequeue();
            command.Execute(state, senderId);
        }
    }
}
