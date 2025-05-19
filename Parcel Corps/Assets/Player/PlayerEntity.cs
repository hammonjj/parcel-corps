using UnityEngine;

public class PlayerEntity : IUpdatableEntity
{
    public string Id { get; private set; }
    public Vector2 Position { get; protected set; }

    private float _speed = 5f;

    public PlayerEntity(string id, Vector2 startPosition)
    {
        Id = id;
        Position = startPosition;
    }

    public virtual void Tick(float deltaTime)
    {
        // Idle logic or status effects could go here in the future
    }

    public void TryMove(Vector2 direction, float deltaTime)
    {
        if (direction.sqrMagnitude > 1f)
        {
            direction = direction.normalized;
        }

        Position += direction * _speed * deltaTime;
    }
}
