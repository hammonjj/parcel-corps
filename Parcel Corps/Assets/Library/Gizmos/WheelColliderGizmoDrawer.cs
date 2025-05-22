using UnityEngine;

[ExecuteAlways]
public class WheelColliderGizmoDrawer : MonoBehaviourBase
{
    private WheelCollider wheel;

    protected override void Awake()
    {
        base.Awake();
        wheel = GetComponent<WheelCollider>();
    }

    protected override void DrawGizmosSafe()
    {
        if (!showGizmos) { return; }
        if (wheel == null) { wheel = GetComponent<WheelCollider>(); }
        if (wheel == null) { return; }

        DrawWheelGizmo(wheel);
    }

    private void DrawWheelGizmo(WheelCollider wc)
    {
        Vector3 pos = wc.transform.position;
        Quaternion rot = wc.transform.rotation;

        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(pos, rot, Vector3.one);
        Gizmos.DrawWireSphere(Vector3.zero, wc.radius);

        Vector3 suspensionDir = -Vector3.up * wc.suspensionDistance;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Vector3.zero, suspensionDir);
    }
}
