using UnityEngine;

public class SceneInitializer : MonoBehaviourBase
{
    [Header("Prefabs")]
    public GameObject playerPrefab;

    [Header("References")]
    public Transform playerSpawnPoint;

    private GameLoop _gameLoop;

    protected override void Awake()
    {
      base.Awake();
        _gameLoop = FindFirstObjectByType<GameLoop>();

        if (_gameLoop == null)
        {
            LogError("GameLoop not found in scene.");
            return;
        }

        if (playerPrefab == null || playerSpawnPoint == null)
        {
            LogError("Missing player prefab or spawn point.");
            return;
        }

        SpawnLocalPlayer();
    }

    private void SpawnLocalPlayer()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        var networkIdentity = playerGO.GetComponent<NetworkIdentity>();
        if (networkIdentity != null)
        {
            networkIdentity.IsLocal = true;
        }

        var vehicleInputRelay = playerGO.GetComponent<VehicleInputRelay>();
        if (vehicleInputRelay != null)
        {
            vehicleInputRelay.CommandQueue = _gameLoop.CommandQueue;
        }

        var vehicleAuthoring = playerGO.GetComponent<VehicleAuthoring>();
        if (vehicleAuthoring != null)
        {
            vehicleAuthoring.gameState = _gameLoop.State;
        }

        LogInfo("Local player spawned and wired.");
    }
}
