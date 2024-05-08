using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Tutorial;
using TbsFramework.Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.CanvasScaler;

namespace TbsFramework.Units.Abilities
{
    public class AttackAbility : Ability
    {
        public Unit UnitToAttack { get; set; }
        public int UnitToAttackID { get; set; }

        public CombatPreviewUI combatPreviewUI;
        public HashSet<Cell> availableDestinations;


        public override void OnAbilitySelected(CellGrid cellGrid)
        {
            availableDestinations = cellGrid.Cells.Where(c => UnitReference.Cell.GetDistance(c) <= UnitReference.AttackRange).ToHashSet();
            Debug.Log("AttackAbility.OnAbilitySelected: Ability selected, attempting to activate attack.");
            IsSelected = true;

            GetComponent<MoveAbility>().IsSelected = false;

            //Debug.Log($"AttackAbility.OnAbilitySelected: isAttackSelected set to {isAttackSelected}");
            Display(cellGrid);
        }

        public override void OnAbilityDeselected(CellGrid cellGrid)
        {
            //Debug.Log("AttackAbility.OnAbilityDeselected");
            IsSelected = false;
            UnitToAttack = null;
            UnitToAttackID = 0;
        }

        public void StartBattle(Unit attacker, Unit defender)
        {
            var attackerPlayer = FindObjectsOfType<Player>().FirstOrDefault(player => player.PlayerNumber == attacker.PlayerNumber);
            var defenderPlayer = FindObjectsOfType<Player>().FirstOrDefault(player => player.PlayerNumber == defender.PlayerNumber);

            Debug.Log(attackerPlayer.name + " is attacking " + defenderPlayer.name);

            BattleData.CurrentPlayerFighterID = attackerPlayer is HumanPlayer ? attacker.UnitID : defender.UnitID;
            BattleData.CurrentEnemyFighterID = attackerPlayer is AIPlayer ? attacker.UnitID : defender.UnitID;

            BattleData.PlayerIsAttacker = attackerPlayer is HumanPlayer;

            BattleData.AddUnitDataToDictionary(attacker);
            BattleData.AddUnitDataToDictionary(defender);

            BattleData.CurrentPlayerNumber = FindObjectOfType<CellGrid>().CurrentPlayerNumber;
            SceneManager.LoadScene("BattleScene");
        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            if (IsSelected && CanPerform(cellGrid) && UnitReference.IsUnitAttackable(unit, UnitReference.Cell))
            {
                UnitToAttack = unit;
                UnitToAttackID = unit.UnitID;
                StartCoroutine(Act(cellGrid)); // This line starts the Act coroutine
                Debug.Log("Attack!");
            }

            if (!IsSelected)
            {
                Debug.Log("Not selected...");
            }

            if (!CanPerform(cellGrid))
            {
                Debug.Log("Can't perform...");
            }

            if (!UnitReference.IsUnitAttackable(unit, UnitReference.Cell))
            {
                Debug.Log("Not attackable...");
            }
        }

        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            if (cellGrid.CurrentPlayer is HumanPlayer human)
            {
                BattleData.UsedPlayerUnits = human.UnitsUsedThisTurn;
            }
            else
            {
                BattleData.UsedPlayerUnits = 10000;
            }

            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unit.Cell = cellGrid.Cells.FirstOrDefault(cell => cell.CurrentUnits.Contains(unit));
                BattleData.AddUnitDataToDictionary(unit);
            }

            Debug.Log("AttackAbility.Act: Started");
            if (CanPerform(cellGrid) && UnitToAttack != null)
            {
                Debug.Log($"AttackAbility.Act: Attacking unit {UnitToAttack.name}");
                // UnitReference.AttackHandler(UnitToAttack);
                StartBattle(UnitReference, UnitToAttack);
                yield return new WaitForSeconds(0.5f);
                //UnitToAttack = null;
                //UnitToAttackID = 0;
            }
            else
            {
                Debug.Log("AttackAbility.Act: Cannot perform attack. CanPerform: " + CanPerform(cellGrid) + "; Unit: " + (UnitToAttack == null ? " NULL" : UnitToAttack.name));
            }
            yield return null;
        }

        private void ShowCombatPreview(Unit defender)
        {
            int attackerCurrentHP = UnitReference.HitPoints;
            int attackerAttack = UnitReference.AttackFactor;
            int attackerDefense = UnitReference.DefenceFactor;
            int defenderAttack = defender.AttackFactor;
            int defenderDefense = defender.DefenceFactor;

            int damageToDefender = Mathf.Max(0, attackerAttack - defenderDefense);
            int damageToAttacker = Mathf.Max(0, defenderAttack - attackerDefense);

            int attackerProjectedHP = Mathf.Max(0, attackerCurrentHP - damageToAttacker);
            int defenderCurrentHP = defender.HitPoints;
            int defenderProjectedHP = Mathf.Max(0, defenderCurrentHP - damageToDefender);

            combatPreviewUI.UpdateAttackerData(UnitReference.name, attackerCurrentHP, attackerProjectedHP, attackerAttack, attackerDefense);
            combatPreviewUI.UpdateDefenderData(defender.name, defenderCurrentHP, defenderProjectedHP, defenderAttack, defenderDefense);
            combatPreviewUI.gameObject.SetActive(true);
        }

        public override void OnUnitHighlighted(Unit unit, CellGrid cellGrid)
        {
            if (IsSelected && UnitReference.IsUnitAttackable(unit, UnitReference.Cell))
            {
                ShowCombatPreview(unit);
                (unit.Cell as SampleSquare).MarkAsAttackRange();
            }
        }

        public override void OnUnitDehighlighted(Unit unit, CellGrid cellGrid)
        {
            if (!IsSelected)
            {
                return;
            }

            combatPreviewUI.gameObject.SetActive(false);

            if (UnitReference.ActionPoints > 0 && availableDestinations.Contains(unit.Cell))
            {
                (unit.Cell as SampleSquare).MarkAsAttackReachable();
            }
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            bool canPerform = UnitReference.ActionPoints > 0;
            //Debug.Log($"AttackAbility.CanPerform: {canPerform}");
            return canPerform;
        }

        public override void Display(CellGrid cellGrid)
        {
            if (!IsSelected)
            {
                return;
            }

            foreach (var cell in availableDestinations)
            {
                (cell as SampleSquare as SampleSquare).MarkAsAttackReachable();
            }
        }

        public override void CleanUp(CellGrid cellGrid)
        {
            if (!IsSelected)
            {
                return;
            }

            foreach (var cell in availableDestinations)
            {
                cell.UnMark();
            }
        }

        public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {
            if (!IsSelected)
            {
                return;
            }

            if (UnitReference.ActionPoints > 0 && availableDestinations.Contains(cell))
            {
                (cell as SampleSquare).MarkAsAttackRange();
            }
        }

        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            if (!IsSelected)
            {
                return;
            }

            if (UnitReference.ActionPoints > 0 && availableDestinations.Contains(cell))
            {
                (cell as SampleSquare).MarkAsAttackReachable();
            }
        }
    }
}