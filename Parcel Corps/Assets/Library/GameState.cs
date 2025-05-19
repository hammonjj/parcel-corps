using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private readonly Dictionary<string, PlayerEntity> _players = new();
    private readonly List<EnemyEntity> _enemies = new();
    private readonly List<IUpdatableEntity> _worldEntities = new();

    public void RegisterPlayer(string id, PlayerEntity entity)
    {
        if (!_players.ContainsKey(id))
        {
            _players[id] = entity;
        }
    }

    public void UnregisterPlayer(string id)
    {
        if (_players.ContainsKey(id))
        {
            _players.Remove(id);
        }
    }

    public PlayerEntity GetPlayer(string id)
    {
        if (_players.TryGetValue(id, out var player))
        {
            return player;
        }

        return null;
    }

    public void RegisterEnemy(EnemyEntity enemy)
    {
        if (!_enemies.Contains(enemy))
        {
            _enemies.Add(enemy);
        }
    }

    public void RegisterWorldEntity(IUpdatableEntity entity)
    {
        if (!_worldEntities.Contains(entity))
        {
            _worldEntities.Add(entity);
        }
    }

    public void Update(float deltaTime)
    {
        foreach (var player in _players.Values)
        {
            player.Tick(deltaTime);
        }

        foreach (var enemy in _enemies)
        {
            enemy.Tick(deltaTime);
        }

        foreach (var entity in _worldEntities)
        {
            entity.Tick(deltaTime);
        }
    }
}
