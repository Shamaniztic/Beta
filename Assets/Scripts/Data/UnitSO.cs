using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Beta/Unit")]
public class UnitSO : ScriptableObject
{
    public int TotalHealth;
    public GameObject AttackPhasePrefab;
}
