using UnityEngine;

public class OrthographicCameraFollow : MonoBehaviourBase
{
    private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f;

    protected override void Awake()
    {
        base.Awake();

        if (target != null)
        {
            offset = transform.position - target.position;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) { return; }

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
}
