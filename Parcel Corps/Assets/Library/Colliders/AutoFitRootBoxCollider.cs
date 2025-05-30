using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(BoxCollider))]
public class AutoFitRootBoxCollider : MonoBehaviourBase
{
    [Tooltip("Only fits the root BoxCollider; other colliders are ignored.")]
    [SerializeField] private bool _autoFit = true;
    [SerializeField] private Quaternion _rotationOffset = Quaternion.identity;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_autoFit)
        {
            FitCollider();
        }
    }

    private void FitCollider()
    {
        var rootCollider = GetComponent<BoxCollider>();
        var renderers = GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            LogDebug($"[{name}] No renderers found to fit BoxCollider.");
            return;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        // Adjust collider using local space
        rootCollider.center = transform.InverseTransformPoint(bounds.center);
        rootCollider.size = transform.InverseTransformVector(bounds.size);
        
        if(_rotationOffset != Quaternion.identity)
        {
            // Apply rotation offset if specified
            rootCollider.transform.rotation = _rotationOffset * rootCollider.transform.rotation;
        }
    }
}
