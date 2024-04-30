using TbsFramework.Cells;
using UnityEngine;
using TbsFramework.Units;
using System.Linq;
using System.Collections;

public class BattleResultHandler : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

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
        foreach (var unit in FindObjectsOfType<Unit>())
        {
            if (BattleData.UnitDataDictionary.ContainsKey(unit.UnitID))
            {
                unit.transform.position = BattleData.UnitDataDictionary[unit.UnitID].Position;
                unit.Cell = BattleData.UnitDataDictionary[unit.UnitID].Cell;
                unit.Cell.IsTaken = true;
            }
        }
    }

    private void UpdateUnitHealth()
    {
        foreach (var unit in FindObjectsOfType<Unit>())
        {
            if (BattleData.UnitDataDictionary.ContainsKey(unit.UnitID))
            {
                unit.HitPoints = BattleData.UnitDataDictionary[unit.UnitID].UnitHealth;
            }
            else
            {
                unit.HitPoints = unit.TotalHitPoints;
            }
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
