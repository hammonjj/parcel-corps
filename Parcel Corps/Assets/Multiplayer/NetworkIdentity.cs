using UnityEngine;

public class NetworkIdentity : MonoBehaviourBase
{
    [Tooltip("Unique player ID or network authority ID.")]
    public string Id;

    [Tooltip("Marks this as the local player's entity.")]
    public bool IsLocal;

    protected override void Awake()
    {
      base.Awake();
        if (string.IsNullOrWhiteSpace(Id))
    {
      Id = System.Guid.NewGuid().ToString();
      LogDebug($"Auto-generated network ID: {Id}");
    }
    }
}
