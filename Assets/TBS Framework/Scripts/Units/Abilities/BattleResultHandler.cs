using TbsFramework.Cells;
using UnityEngine;
using TbsFramework.Units;
using System.Linq;
using System.Collections;
using TbsFramework.Players;
using TbsFramework.Grid;
using System.Collections.Generic;

public class BattleResultHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToReactivate;

    private List<SpriteRenderer> unitSprites = new List<SpriteRenderer>();

    private IEnumerator Start()
    {
        yield break;

        if (BattleData.CurrentPlayerFighterData == null)
        {
            yield break;
        }

        yield return new WaitForEndOfFrame();

        FindObjectOfType<CellGrid>().SetCurrentPlayerNumber(BattleData.CurrentPlayerNumber);
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

    public void ExecuteBattleResults(AsyncOperation op)
    {
        StartCoroutine(BattleResultsCoroutine(op));
    }

    private IEnumerator BattleResultsCoroutine(AsyncOperation op)
    {
        yield return new WaitUntil(() => op.isDone);

        foreach (var obj in objectsToReactivate)
        {
            obj.SetActive(true);
        }

        if (unitSprites == null || unitSprites.Count == 0)
        {
            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unitSprites.Add(unit.GetComponent<SpriteRenderer>());
            }
        }

        foreach (var sprite in unitSprites)
        {
            sprite.enabled = true;
        }

        if (BattleData.CurrentPlayerFighterData == null)
        {
            yield break;
        }

        FindObjectOfType<CellGrid>().SetCurrentPlayerNumber(BattleData.CurrentPlayerNumber);
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

    public void DeactivateObjects()
    {
        foreach (var obj in objectsToReactivate)
        {
            obj.SetActive(false);
        }

        if (unitSprites == null || unitSprites.Count == 0)
        {
            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unitSprites.Add(unit.GetComponent<SpriteRenderer>());
            }
        }

        foreach (var sprite in unitSprites)
        {
            sprite.enabled = false;
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

                Debug.LogError(unit.UName + " has done turn: " + BattleData.UnitDataDictionary[unit.UnitID].HasDoneTurn);
                
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
                if (BattleData.UnitDataDictionary[unit.UnitID].UnitHealth <= 0)
                {
                    unitSprites.Remove(unit.GetComponent<SpriteRenderer>());
                    unit.DefendHandler(unit, 1000);
                    continue;
                }

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
