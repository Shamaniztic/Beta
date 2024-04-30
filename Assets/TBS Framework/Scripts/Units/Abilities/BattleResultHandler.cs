using TbsFramework.Cells;
using UnityEngine;
using TbsFramework.Units;

public class BattleResultHandler : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("BattleResultHandler.Start: Restoring unit position.");
        RestoreUnitPosition();
        UpdateUnitHealth();

        if (BattleData.PlayerWon.HasValue)
        {
            Debug.Log("BattleResultHandler.Start: Handling battle result. Player won: " + BattleData.PlayerWon.Value);
            HandleBattleResult(BattleData.PlayerWon.Value);
            BattleData.PlayerWon = null;
        }
        else
        {
            Debug.Log("BattleResultHandler.Start: No battle result to handle.");
        }
    }

    private void RestoreUnitPosition()
    {
        int unitID = BattleData.PlayerUnitID;
        float posX = PlayerPrefs.GetFloat($"{unitID}_PosX");
        float posY = PlayerPrefs.GetFloat($"{unitID}_PosY");
        float posZ = PlayerPrefs.GetFloat($"{unitID}_PosZ");
        float cellX = PlayerPrefs.GetFloat($"{unitID}_CellX");
        float cellY = PlayerPrefs.GetFloat($"{unitID}_CellY");

        GameObject playerUnit = GameObject.Find(BattleData.PlayerUnitName);
        Cell destinationCell = FindCellByCoordinates(cellX, cellY);

        if (playerUnit != null && destinationCell != null)
        {
            playerUnit.transform.position = new Vector3(posX, posY, posZ);
            Unit playerUnitComponent = playerUnit.GetComponent<Unit>();
            playerUnitComponent.Cell = destinationCell;
            destinationCell.IsTaken = true;
        }
        else
        {
            Debug.LogError("BattleResultHandler.RestoreUnitPosition: Player unit or destination cell not found.");
        }
    }

    private void UpdateUnitHealth()
    {
        GameObject playerUnit = GameObject.Find(BattleData.PlayerUnitName);
        if (playerUnit != null)
        {
            Unit playerUnitComponent = playerUnit.GetComponent<Unit>();
            int previousPlayerHealth = playerUnitComponent.HitPoints;
            playerUnitComponent.HitPoints = BattleData.PlayerUnitHealth;
            Debug.Log($"Player unit health updated - Previous: {previousPlayerHealth}, Current: {playerUnitComponent.HitPoints}", playerUnit);
        }

        GameObject enemyUnit = GameObject.Find(BattleData.EnemyUnitName);
        if (enemyUnit != null)
        {
            Unit enemyUnitComponent = enemyUnit.GetComponent<Unit>();
            int previousEnemyHealth = enemyUnitComponent.HitPoints;
            enemyUnitComponent.HitPoints = BattleData.EnemyUnitHealth;
            Debug.Log($"Enemy unit health updated - Previous: {previousEnemyHealth}, Current: {enemyUnitComponent.HitPoints}", enemyUnit);
        }
    }

    private Cell FindCellByCoordinates(float x, float y)
    {
        foreach (Cell cell in FindObjectsOfType<Cell>())
        {
            if (cell.OffsetCoord.x == x && cell.OffsetCoord.y == y)
            {
                return cell;
            }
        }
        return null;
    }

    private void HandleBattleResult(bool playerWon)
    {
        Debug.Log($"BattleResultHandler.HandleBattleResult: Player won: {playerWon}");
        // Additional battle result handling
    }
}
