using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players;
using UnityEngine;

namespace TbsFramework.Units.Abilities
{
    public class MoveAbility : Ability
    {
        public Cell Destination { get; set; }
        private IList<Cell> currentPath;
        public HashSet<Cell> availableDestinations;
        private ActionMenu actionMenu;

        private void SaveMovementLocation(CellGrid cellGrid)
        {
            UnitReference.Cell = Destination;
            BattleData.AddUnitDataToDictionary(UnitReference);
        }

        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            if (UnitReference.ActionPoints > 0 && availableDestinations.Contains(Destination))
            {
                var path = UnitReference.FindPath(cellGrid.Cells, Destination);
                yield return UnitReference.Move(Destination, path);

                // Save the movement location
                SaveMovementLocation(cellGrid);

                // Trigger the popup after the move is complete, only for the human player
                if (UnitReference.PlayerNumber == 0) // Assuming player number 0 is the human player
                {
                    var actionMenu = FindObjectOfType<ActionMenu>();
                    if (actionMenu != null)
                    {
                        actionMenu.SetMoveAbility(this);
                        actionMenu.SetCellGrid(cellGrid);
                        actionMenu.SetPosition(UnitReference.transform.position); // Pass the unit's position
                        actionMenu.ShowActionMenu();
                    }
                }
            }
            yield return base.Act(cellGrid, isNetworkInvoked);
        }

        public void ShowActionMenu(CellGrid cellGrid)
        {
            IsSelected = false;
            actionMenu = FindObjectOfType<ActionMenu>();
            if (actionMenu != null)
            {
                actionMenu.SetMoveAbility(this);
                actionMenu.SetCellGrid(cellGrid);
                actionMenu.ShowActionMenu();
            }
        }

        public void AttackSelectedEnemy(CellGrid cellGrid)
        {
            var attackAbility = UnitReference.GetComponent<AttackAbility>();
            if (attackAbility != null && attackAbility.CanPerform(cellGrid))
            {
                attackAbility.OnAbilitySelected(cellGrid);
            }
        }

        public override void Display(CellGrid cellGrid)
        {
            if (!IsSelected)
            {
                return;
            }

            if (UnitReference.ActionPoints > 0)
            {
                foreach (var cell in availableDestinations)
                {
                    cell.MarkAsReachable();
                }
            }
        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            if (IsSelected)
            {
                return;
            }

            IsSelected = true;

            if (cellGrid.GetCurrentPlayerUnits().Contains(unit))
            {
                cellGrid.cellGridState = new CellGridStateAbilitySelected(cellGrid, unit, unit.GetComponents<Ability>().ToList());
            }
        }

        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            if (!IsSelected)
            {
                return;
            }

            if (availableDestinations.Contains(cell))
            {
                Destination = cell;
                currentPath = null;
                StartCoroutine(HumanExecute(cellGrid));
            }
            else
            {
                // Keep the unit selected if it's the current player's unit
                if (cellGrid.GetCurrentPlayerUnits().Contains(UnitReference))
                {
                    cellGrid.cellGridState = new CellGridStateAbilitySelected(cellGrid, UnitReference, UnitReference.GetComponents<Ability>().ToList());
                }
                else
                {
                    cellGrid.cellGridState = new CellGridStateWaitingForInput(cellGrid);
                }
            }
        }

        public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {
            if (!IsSelected || FindObjectOfType<HumanPlayer>().CurrentUnit != UnitReference)
            {
                return;
            }

            if (UnitReference.ActionPoints > 0 && availableDestinations.Contains(cell))
            {
                currentPath = UnitReference.FindPath(cellGrid.Cells, cell);
                foreach (var c in currentPath)
                {
                    c.MarkAsPath();
                }
            }
        }

        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            if (!IsSelected || FindObjectOfType<HumanPlayer>().CurrentUnit != UnitReference)
            {
                return;
            }

            if (UnitReference.ActionPoints > 0 && availableDestinations.Contains(cell))
            {
                if (currentPath == null)
                {
                    return;
                }
                foreach (var c in currentPath)
                {
                    c.MarkAsReachable();
                }
            }
        }

        public override void OnAbilitySelected(CellGrid cellGrid)
        {
            if (GetComponent<AttackAbility>().IsSelected)
            {
                return;
            }

            UnitReference.CachePaths(cellGrid.Cells);
            availableDestinations = UnitReference.GetAvailableDestinations(cellGrid.Cells);

            IsSelected = true;
            Display(cellGrid);
        }

        public override void CleanUp(CellGrid cellGrid)
        {
            foreach (var cell in availableDestinations)
            {
                cell.UnMark();
            }
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            return UnitReference.ActionPoints > 0 && UnitReference.GetAvailableDestinations(cellGrid.Cells).Count > 0;
        }

        public override IDictionary<string, string> Encapsulate()
        {
            var actionParams = new Dictionary<string, string>();

            actionParams.Add("destination_x", Destination.OffsetCoord.x.ToString());
            actionParams.Add("destination_y", Destination.OffsetCoord.y.ToString());

            return actionParams;
        }

        public override IEnumerator Apply(CellGrid cellGrid, IDictionary<string, string> actionParams, bool isNetworkInvoked = false)
        {
            var actionDestination = cellGrid.Cells.Find(c => c.OffsetCoord.Equals(new UnityEngine.Vector2(float.Parse(actionParams["destination_x"]), float.Parse(actionParams["destination_y"]))));
            Destination = actionDestination;
            yield return StartCoroutine(RemoteExecute(cellGrid));
        }
    }
}