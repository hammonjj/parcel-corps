using UnityEngine;

public class PlayerAuthoring : MonoBehaviourBase
{
    public string playerId;
    public GameState gameState;

    private void Start()
    {
        var playerEntity = new PlayerEntity(playerId, transform.position);
        gameState.RegisterPlayer(playerId, playerEntity);
    }

    private void Update()
    {
        var player = gameState.GetPlayer(playerId);
        if (player != null)
        {
            transform.position = player.Position;
        }
    }
}
