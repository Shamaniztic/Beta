using UnityEngine;
using TbsFramework.Units;

public class BattleUnit : MonoBehaviour
{
    public Unit Unit { get; private set; }
    public int CurrentHitPoints { get; private set; }

    private void Awake()
    {
        Unit = GetComponent<Unit>();
        CurrentHitPoints = Unit.HitPoints;
    }

    public void TakeDamage(int damage)
    {
        CurrentHitPoints = Mathf.Max(0, CurrentHitPoints - damage);
    }

    public bool IsDefeated()
    {
        return CurrentHitPoints <= 0;
    }
}