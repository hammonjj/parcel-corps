using UnityEngine;

public class EnemyEntity
{
    public Vector2 Position { get; private set; }

    private float _wanderRadius = 1f;
    private float _speed = 2f;
    private float _wanderTimer = 0f;

    public EnemyEntity(Vector2 startPosition)
    {
        Position = startPosition;
    }

    public void Tick(float deltaTime)
    {
        _wanderTimer -= deltaTime;

        if (_wanderTimer <= 0f)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            Position += randomDir * _speed * deltaTime;
            _wanderTimer = Random.Range(1f, 3f);
        }
    }
}
