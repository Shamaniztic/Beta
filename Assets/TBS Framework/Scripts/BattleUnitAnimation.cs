using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Player, Enemy }

public class BattleUnitAnimation : MonoBehaviour
{
    [SerializeField] private UnitType unitType;

    // This is called through an animation event
    public void ConnectAttack()
    {
        BattleManager.Instance.ConnectAttack(unitType);
    }
}
