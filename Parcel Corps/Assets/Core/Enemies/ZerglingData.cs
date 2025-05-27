using UnityEngine;

[CreateAssetMenu(fileName = "ZerglingData", menuName = "Scriptable Objects/ZerglingData")]
public class ZerglingData : ScriptableObject
{
    public float MovementSpeed = 5f;
    public float AttackDamage = 10f;
    public float AttackRange = 1.5f;
    public float AttackCooldown = 1f;
    public float Health = 50f;
}
