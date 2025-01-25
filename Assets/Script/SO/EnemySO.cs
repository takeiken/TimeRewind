using UnityEngine;

[System.Serializable]
public class Enemy
{
    public string Name;
    public float ReactLength;
    public float ReactAngle;
    public float MovementSpeed;
    public float AttackRange;
    public float AttackFrequency;
    public float Armor;
}

[CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public Enemy Enemy;
}
