using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(WheelCollider))]
public class AutoFitWheelCollider : MonoBehaviourBase
{
    [Tooltip("Auto-fit the WheelCollider radius and vertical position from mesh bounds.")]
    [SerializeField] private bool _autoFit = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_autoFit)
        {
            FitWheelCollider();
        }
    }

    [ContextMenu("Fit WheelCollider")]
    public void FitWheelCollider()
    {
        var wheel = GetComponent<WheelCollider>();
        var meshFilter = GetComponent<MeshFilter>();

        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            LogWarning($"[{name}] No valid MeshFilter found to fit WheelCollider.");
            return;
        }

        var bounds = meshFilter.sharedMesh.bounds;
        var scale = meshFilter.transform.lossyScale;

        float radius = bounds.extents.y * scale.y;
        wheel.radius = radius;

        // Match center based on local Y offset of mesh
        Vector3 meshWorldCenter = meshFilter.transform.TransformPoint(bounds.center);
        Vector3 localCenter = transform.InverseTransformPoint(meshWorldCenter);
        wheel.center = new Vector3(localCenter.x, localCenter.y, localCenter.z);

        LogDebug($"[{name}] Fitted WheelCollider → Radius: {radius:F2}, Center: {wheel.center}");
    }
}
