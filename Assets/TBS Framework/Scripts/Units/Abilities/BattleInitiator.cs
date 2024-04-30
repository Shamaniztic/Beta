using UnityEngine;
using TbsFramework.Units;
using UnityEngine.SceneManagement;

public class BattleInitiator : MonoBehaviour
{
    public void StartBattle(Unit playerUnit, Unit enemyUnit)
    {
        // Store the unit IDs, names, health, and player numbers in BattleData
        BattleData.PlayerUnitID = playerUnit.UnitID;
        BattleData.EnemyUnitID = enemyUnit.UnitID;
        BattleData.PlayerUnitName = playerUnit.UName;
        BattleData.EnemyUnitName = enemyUnit.UName;
        BattleData.PlayerUnitPlayerNumber = playerUnit.PlayerNumber;
        BattleData.EnemyUnitPlayerNumber = enemyUnit.PlayerNumber;
        BattleData.PlayerUnitHealth = playerUnit.HitPoints;
        BattleData.EnemyUnitHealth = enemyUnit.HitPoints;

        // Store references to the player unit and enemy unit in BattleData
        BattleData.PlayerUnit = playerUnit;
        BattleData.EnemyUnit = enemyUnit;

        // Load the battle scene
        SceneManager.LoadScene("BattleScene");
    }
}