using UnityEngine;

public class VehicleAuthoring : MonoBehaviourBase
{
  public string playerId;
  public GameState gameState;
  public VehicleConfig config;

  private void Start()
  {
    var vehicle = new VehiclePlayerEntity(playerId, transform.position, config);

    gameState??= FindFirstObjectByType<GameLoop>()?.State;
    gameState.RegisterPlayer(playerId, vehicle);
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
