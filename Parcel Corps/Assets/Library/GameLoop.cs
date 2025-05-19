using UnityEngine;

public class GameLoop : MonoBehaviourBase
{
    public GameState State = new();
    public CommandQueue CommandQueue = new();

    public InputRouter InputRouter;

    private void Update()
    {
        CommandQueue.Flush(State);
        State.Update(Time.deltaTime);
    }
}
