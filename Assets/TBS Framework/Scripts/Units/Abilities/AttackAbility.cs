using System.Collections;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TbsFramework.Units.Abilities
{
    public class AttackAbility : Ability
    {
        public Unit UnitToAttack { get; set; }
        public int UnitToAttackID { get; set; }

        public bool isAttackSelected;

        public CombatPreviewUI combatPreviewUI;

        public override void OnAbilitySelected(CellGrid cellGrid)
        {
            Debug.Log("AttackAbility.OnAbilitySelected: Ability selected, attempting to activate attack.");
            isAttackSelected = true;
            Debug.Log($"AttackAbility.OnAbilitySelected: isAttackSelected set to {isAttackSelected}");
        }

        public override void OnAbilityDeselected(CellGrid cellGrid)
        {
            Debug.Log("AttackAbility.OnAbilityDeselected");
            isAttackSelected = false;
            UnitToAttack = null;
            UnitToAttackID = 0;
        }

        public void StartBattle(Unit attacker, Unit defender)
        {
            Debug.Log(attacker.gameObject.name + " VS " + defender.gameObject.name);
            BattleData.CurrentPlayerFighterID = attacker.UnitID;
            BattleData.CurrentEnemyFighterID = defender.UnitID;

            BattleData.AddUnitDataToDictionary(attacker);
            BattleData.AddUnitDataToDictionary(defender);

            SceneManager.LoadScene("BattleScene");
        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            if (isAttackSelected && CanPerform(cellGrid) && UnitReference.IsUnitAttackable(unit, UnitReference.Cell))
            {
                UnitToAttack = unit;
                UnitToAttackID = unit.UnitID;
                StartCoroutine(Act(cellGrid)); // This line starts the Act coroutine
            }
        }

        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            Debug.Log("AttackAbility.Act: Started");
            if (CanPerform(cellGrid) && UnitToAttack != null)
            {
                Debug.Log($"AttackAbility.Act: Attacking unit {UnitToAttack.name}");
                UnitReference.AttackHandler(UnitToAttack);
                yield return new WaitForSeconds(0.5f);
                UnitToAttack = null;
                UnitToAttackID = 0;
            }
            else
            {
                Debug.Log("AttackAbility.Act: Cannot perform attack");
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
            if (isAttackSelected && UnitReference.IsUnitAttackable(unit, UnitReference.Cell))
            {
                ShowCombatPreview(unit);
            }
        }

        public override void OnUnitDehighlighted(Unit unit, CellGrid cellGrid)
        {
            combatPreviewUI.gameObject.SetActive(false);
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            bool canPerform = UnitReference.ActionPoints > 0;
            Debug.Log($"AttackAbility.CanPerform: {canPerform}");
            return canPerform;
        }
    }
}