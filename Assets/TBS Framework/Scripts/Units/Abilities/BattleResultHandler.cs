using TbsFramework.Cells;
using UnityEngine;
using TbsFramework.Units;
using System.Linq;
using System.Collections;
using TbsFramework.Players;
using TbsFramework.Grid;

public class BattleResultHandler : MonoBehaviour
{
    private IEnumerator Start()
    {
        if (BattleData.CurrentPlayerFighterData == null)
        {
            yield break;
        }

        yield return new WaitForEndOfFrame();

        Debug.Log("BattleResultHandler.Start: Restoring unit position.");
        RestoreUnitPosition();
        UpdateUnitHealth();
        FindObjectOfType<HumanPlayer>().SetUnitsUsedValue(BattleData.UsedPlayerUnits);

        if (BattleData.PlayerWon.HasValue)
        {
            Debug.Log("BattleResultHandler.Start: Handling battle result. Player won: " + BattleData.PlayerWon.Value);
            HandleBattleResult(BattleData.PlayerWon.Value);
            BattleData.PlayerWon = null;
        }
        else
        {
            Debug.Log("BattleResultHandler.Start: No battle result to handle.");
            FindObjectOfType<CellGrid>().EndTurn();
        }
    }

    private void RestoreUnitPosition()
    {
        foreach (var unit in FindObjectsOfType<Unit>())
        {
            if (BattleData.UnitDataDictionary.ContainsKey(unit.UnitID))
            {
                unit.transform.position = BattleData.UnitDataDictionary[unit.UnitID].Position;
                unit.Cell = FindObjectsOfType<Cell>().FirstOrDefault(cell => cell.OffsetCoord == BattleData.UnitDataDictionary[unit.UnitID].CellOffset);
                unit.Cell.IsTaken = true;
                
                if (BattleData.UnitDataDictionary[unit.UnitID].HasDoneTurn)
                {
                    unit.SetTurnDone();
                }
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
