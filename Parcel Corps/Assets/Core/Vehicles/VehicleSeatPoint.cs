using UnityEngine;

[System.Serializable]
public class VehicleSeatPoint
{
    public string SeatName;
    public Transform SeatTransform;
    public Transform LoadingTransform;
    public Transform FireTransform;
    public bool IsPassenger;
    public bool IsDriver;
}
