using UnityEngine;

[CreateAssetMenu(fileName = "BaseVehicleData", menuName = "Vehicles/BaseVehicleData")]
public class BaseVehicleData : ScriptableObject
{
    public string vehicleName = "Base Vehicle";
    public float maxMotorTorque = 1500f;
    public float maxSteeringAngle = 30f;
    public float brakeTorque = 3000f;
    public float maxSpeed = 100f;
}
