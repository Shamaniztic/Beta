using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TbsFramework.Units.UnitStates;
using UnityEngine;

namespace TbsFramework.Grid.GridStates
{
    public class CellGridStateAbilitySelected : CellGrid.CellGridState
    {
        List<Ability> _abilities;
        Unit _unit;
        private bool _isAttackModeActive;

        public CellGridStateAbilitySelected(CellGrid cellGrid, Unit unit, List<Ability> abilities) : base(cellGrid)
        {
            if (abilities.Count == 0)
            {
                Debug.LogError("No abilities were selected, check if your unit has any abilities attached to it");
            }

            _abilities = abilities;
            _unit = unit;
            _isAttackModeActive = false;
        }

        public CellGridStateAbilitySelected(CellGrid cellGrid, Unit unit, Ability ability) : this(cellGrid, unit, new List<Ability>() { ability }) { }

        public void SetAttackModeActive(bool isActive)
        {
            _isAttackModeActive = isActive;
        }

        public override void OnUnitClicked(Unit unit)
        {
            Debug.Log($"CellGridStateAbilitySelected.OnUnitClicked: Clicked on unit {unit.UnitID}");
            base.OnUnitClicked(unit);

            _abilities.ForEach(a => a.OnUnitClicked(unit, _cellGrid));
            return;

            var attackAbility = _unit.GetComponent<AttackAbility>();
            if (attackAbility != null)
            {
                Debug.Log($"AttackAbility found. isAttackSelected: {attackAbility.IsSelected}, IsUnitAttackable: {_unit.IsUnitAttackable(unit, _unit.Cell)}");

                if (_unit != unit && _isAttackModeActive && _unit.IsUnitAttackable(unit, _unit.Cell))
                {
                    Debug.Log("CellGridStateAbilitySelected.OnUnitClicked: Attack initiated.");
                    attackAbility.UnitToAttack = unit;
                    attackAbility.UnitToAttackID = unit.UnitID;

                    // Start the battle after the attack is initiated
                    attackAbility.StartBattle(_unit, unit);

                    _cellGrid.StartCoroutine(attackAbility.Act(_cellGrid));

                    // Reset the attack mode flag and exit the state after the attack is performed
                    SetAttackModeActive(false);
                    _cellGrid.cellGridState = new CellGridStateWaitingForInput(_cellGrid);
                }
                else
                {
                    Debug.Log("CellGridStateAbilitySelected.OnUnitClicked: Attack conditions not met, handling with default abilities.");
                    _abilities.ForEach(a => a.OnUnitClicked(unit, _cellGrid));

                    // Reset the attack mode flag and exit the state if the attack conditions are not met
                    SetAttackModeActive(false);
                    //_cellGrid.cellGridState = new CellGridStateWaitingForInput(_cellGrid);
                }
            }
            else
            {
                Debug.Log("AttackAbility component not found on unit.");
                _abilities.ForEach(a => a.OnUnitClicked(unit, _cellGrid));
            }
        }

        public override void OnUnitHighlighted(Unit unit)
        {
            // Debug.Log($"CellGridStateAbilitySelected.OnUnitHighlighted: Highlighting unit {unit.UnitID}");
            _abilities.ForEach(a => a.OnUnitHighlighted(unit, _cellGrid));
        }

        public override void OnUnitDehighlighted(Unit unit)
        {
            // Debug.Log($"CellGridStateAbilitySelected.OnUnitDehighlighted: Dehighlighting unit {unit.UnitID}");
            _abilities.ForEach(a => a.OnUnitDehighlighted(unit, _cellGrid));
        }

        public override void OnCellClicked(Cell cell)
        {
            // Debug.Log($"CellGridStateAbilitySelected.OnCellClicked: Clicked on cell at {cell.OffsetCoord}");
            _abilities.ForEach(a => a.OnCellClicked(cell, _cellGrid));
        }

        public override void OnCellSelected(Cell cell)
        {
            // Debug.Log($"CellGridStateAbilitySelected.OnCellSelected: Cell at {cell.OffsetCoord} selected");
            base.OnCellSelected(cell);
            _abilities.ForEach(a => a.OnCellSelected(cell, _cellGrid));
        }

        public override void OnCellDeselected(Cell cell)
        {
            // Debug.Log($"CellGridStateAbilitySelected.OnCellDeselected: Cell at {cell.OffsetCoord} deselected");
            base.OnCellDeselected(cell);
            _abilities.ForEach(a => a.OnCellDeselected(cell, _cellGrid));
        }

        public override void OnStateEnter()
        {
            //Debug.Log("CellGridStateAbilitySelected.OnStateEnter: State entered, checking ability performance");
            _unit?.OnUnitSelected();
            _abilities.ForEach(a => 
            {
                if (a is MoveAbility)
                {
                    a.OnAbilitySelected(_cellGrid); 
                }
            });


            _abilities.ForEach(a =>
            {
                if (a is MoveAbility)
                {
                    a.Display(_cellGrid);
                }
            });

            var canPerformAction = _abilities.Select(a => a.CanPerform(_cellGrid))
                                             .DefaultIfEmpty()
                                             .Aggregate((result, next) => result || next);
            if (!canPerformAction)
            {
                // Debug.Log("CellGridStateAbilitySelected.OnStateEnter: No abilities can perform, marking unit as finished");
                _unit?.SetState(new UnitStateMarkedAsFinished(_unit));
            }
        }

        public override void OnStateExit()
        {
            // Debug.Log("CellGridStateAbilitySelected.OnStateExit: State exiting, cleaning up");
            _unit?.OnUnitDeselected();
            _abilities.ForEach(a => a.OnAbilityDeselected(_cellGrid));
            _abilities.ForEach(a => a.CleanUp(_cellGrid));
        }
    }
}