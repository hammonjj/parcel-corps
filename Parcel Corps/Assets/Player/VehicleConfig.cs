using UnityEngine;

[CreateAssetMenu(fileName = "VehicleConfig", menuName = "Game/Vehicle Config")]
public class VehicleConfig : ScriptableObject
{
    [Header("Motion")]
    [Range(1f, 100f)]
    public float maxSpeed = 15f;

    [Range(1f, 100f)]
    public float acceleration = 25f;

    [Range(1f, 100f)]
    public float brakeForce = 40f;

    [Range(1f, 360f)]
    public float turnRate = 90f;
}