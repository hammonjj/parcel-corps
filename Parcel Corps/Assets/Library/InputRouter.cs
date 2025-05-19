using System.Collections.Generic;
using UnityEngine;

public class InputRouter : MonoBehaviourBase
{
    public string LocalPlayerId = "player_1";
    private readonly Queue<IPlayerCommand> _pendingCommands = new();

    public void EnqueueCommand(IPlayerCommand command)
    {
        _pendingCommands.Enqueue(command);
    }

    public void ProcessCommands(GameState state)
    {
        while (_pendingCommands.Count > 0)
        {
            IPlayerCommand command = _pendingCommands.Dequeue();
            command.Execute(state, LocalPlayerId);
        }
    }
}
