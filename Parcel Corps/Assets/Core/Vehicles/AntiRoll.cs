using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    [SerializeField] private float _antiRollForce = 5000f;
    [SerializeField] private WheelCollider _frontLeft, _frontRight;
    [SerializeField] private WheelCollider _rearLeft, _rearRight;
    [SerializeField] private float _maxLeanAngle = 30f;
    [SerializeField] private float _leanRecoveryStrength = 2000f;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // Lower your COM a bit to make rollover harder:
        //_rb.centerOfMass += Vector3.down * 0.5f;
    }

    private void FixedUpdate()
    {
        ApplyAntiRoll(_frontLeft, _frontRight);
        ApplyAntiRoll(_rearLeft, _rearRight);
        
        ApplyLeanClamping();
    }

    private void ApplyLeanClamping()
    { 
        // Measure current roll around the vehicle's forward axis
        float lean = Vector3.SignedAngle(transform.up, Vector3.up, transform.forward);

        if (Mathf.Abs(lean) > _maxLeanAngle)
        {
            // How far past the limit?
            float excess = lean - Mathf.Sign(lean) * _maxLeanAngle;

            // Push it back
            _rb.AddTorque(transform.forward * -excess * _leanRecoveryStrength, ForceMode.Acceleration);
        }
    }

    private void ApplyAntiRoll(WheelCollider left, WheelCollider right)
    {
        left.GetGroundHit(out var hitL);
        right.GetGroundHit(out var hitR);

        float travelL = 1f, travelR = 1f;
        if (hitL.collider != null)
        {
            travelL = (-left.transform.InverseTransformPoint(hitL.point).y - left.radius)
                / left.suspensionDistance;
        }
        if (hitR.collider != null)
        {
            travelR = (-right.transform.InverseTransformPoint(hitR.point).y - right.radius)
                / right.suspensionDistance;
        }

        float antiRoll = (travelL - travelR) * _antiRollForce;

        Vector3 chassisUp = _rb.transform.up;

        if (hitL.collider != null)
        {
            _rb.AddForceAtPosition(chassisUp * -antiRoll, left.transform.position, ForceMode.Force);
        }
        if (hitR.collider != null)
        {
            _rb.AddForceAtPosition(chassisUp * antiRoll, right.transform.position, ForceMode.Force);
        }
    }
}
