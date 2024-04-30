using UnityEngine;
using TbsFramework.Units;
using UnityEngine.SceneManagement;

public class BattleInitiator : MonoBehaviour
{
    public void StartBattle(Unit playerUnit, Unit enemyUnit)
    {
        // Store the unit IDs, names, health, and player numbers in BattleData
        BattleData.CurrentPlayerFighterID = playerUnit.UnitID;
        BattleData.CurrentEnemyFighterID = enemyUnit.UnitID;

        BattleData.AddUnitDataToDictionary(playerUnit);
        BattleData.AddUnitDataToDictionary(enemyUnit);

        // Load the battle scene
        SceneManager.LoadScene("BattleScene");
    }
}