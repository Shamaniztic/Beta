using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TbsFramework.Units;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private RectTransform canvasParent;
    [SerializeField] private HealthBar playerHealthBar;

    [Header("Debug")]
    [SerializeField] private bool useDebug = true;
    [SerializeField] private UnitSO debugPlayerUnit;
    [SerializeField] private UnitSO debugEnemyUnit;

    private void Start()
    {
        if (useDebug)
        {
            Instantiate(debugPlayerUnit.AttackPhasePrefab, canvasParent);
            Instantiate(debugEnemyUnit.AttackPhasePrefab, canvasParent);
        }
        else
        {
            Instantiate(BattleData.CurrentPlayerFighterData.Data.AttackPhasePrefab, canvasParent);
            Instantiate(BattleData.CurrentEnemyFighterData.Data.AttackPhasePrefab, canvasParent);
        }

        // Debug logs for initial health values
        Debug.Log($"BattleManager.Start: Player unit '{BattleData.CurrentPlayerFighterData.UnitName}' initial health: {BattleData.CurrentPlayerFighterData.UnitHealth}");
        Debug.Log($"BattleManager.Start: Enemy unit '{BattleData.CurrentEnemyFighterData.UnitName}' initial health: {BattleData.CurrentEnemyFighterData.UnitHealth}");

        // Start the battle sequence
        StartCoroutine(BattleSequence());
    }

    [System.Serializable]
    public class PlayerUnitPrefabEntry
    {
        public string unitName;
        public GameObject prefab;
    }

    private IEnumerator BattleSequence()
    {
        BattleData.CurrentPlayerFighterData.LoseHealth(1);
        BattleData.CurrentEnemyFighterData.LoseHealth(1);

        yield return new WaitForSeconds(2);

        yield return StartCoroutine(EndBattle(null));
        /*x
        // Debug: Log the initial health of both units
        Debug.Log($"Initial health - Player: {playerUnit.HitPoints}, Enemy: {enemyUnit.HitPoints}");

        // Play the player unit's attack animation
        yield return StartCoroutine(PlayUnitAttackAnimation(playerUnit));

        // Debug: Log the health before the player's attack
        Debug.Log($"Before player's attack - Player: {playerUnit.HitPoints}, Enemy: {enemyUnit.HitPoints}");

        // Player unit attacks the enemy unit
        playerUnit.AttackHandler(enemyUnit);

        // Debug: Log the health after the player's attack
        Debug.Log($"After player's attack - Player: {playerUnit.HitPoints}, Enemy: {enemyUnit.HitPoints}");

        // Check if the enemy unit is defeated
        if (IsUnitDefeated(enemyUnit))
        {
            // Enemy unit is defeated, end the battle
            yield return StartCoroutine(EndBattle(true));
            yield break;
        }

        // Play the enemy unit's attack animation
        yield return StartCoroutine(PlayUnitAttackAnimation(enemyUnit));

        // Debug: Log the health before the enemy's attack
        Debug.Log($"Before enemy's attack - Player: {playerUnit.HitPoints}, Enemy: {enemyUnit.HitPoints}");

        // Enemy unit attacks the player unit
        enemyUnit.AttackHandler(playerUnit);

        // Debug: Log the health after the enemy's attack
        Debug.Log($"After enemy's attack - Player: {playerUnit.HitPoints}, Enemy: {enemyUnit.HitPoints}");

        // Check if the player unit is defeated
        if (IsUnitDefeated(playerUnit))
        {
            // Player unit is defeated, end the battle
            yield return StartCoroutine(EndBattle(false));
            yield break;
        }

        yield return StartCoroutine(EndBattle(null));
        // Battle sequence finished, transition back to the main scene
        TransitionToMainScene();
        */
    }

    private IEnumerator PlayUnitAttackAnimation(Unit unit)
    {
        // Play the attack animation of the unit
        // You can access the Animator component of the unit and trigger the attack animation
        // Example:
        // unit.GetComponent<Animator>().SetTrigger("Attack");

        // Wait for the animation to finish
        // You can use a fixed duration or a more sophisticated way to determine when the animation is complete
        yield return new WaitForSeconds(1f);
    }

    private int CalculateDamage(Unit attacker, Unit defender)
    {
        int damage = Mathf.Max(0, attacker.AttackFactor - defender.DefenceFactor);
        return damage;
    }

    private void ApplyDamage(Unit unit, int damage)
    {
        /*
        unit.HitPoints = Mathf.Max(0, unit.HitPoints - damage);

        // Update the health bars
        if (unit == playerUnit)
        {
            healthBar.UpdatePlayerHealth(playerUnit.HitPoints);
        }
        else if (unit == enemyUnit)
        {
            healthBar.UpdateEnemyHealth(enemyUnit.HitPoints);
        }
        */
    }

    private bool IsUnitDefeated(Unit unit)
    {
        return unit.HitPoints <= 0;
    }

    private IEnumerator EndBattle(bool? playerWon)
    {
        // Debug logs for final health values
        Debug.Log($"BattleManager.EndBattle: Player unit '{BattleData.CurrentPlayerFighterData.UnitHealth}' final health: {BattleData.CurrentPlayerFighterData.UnitHealth}");
        Debug.Log($"BattleManager.EndBattle: Enemy unit '{BattleData.CurrentPlayerFighterData.UnitHealth}' final health: {BattleData.CurrentPlayerFighterData.UnitHealth}");

        // Perform any necessary actions based on the battle outcome
        // Example: Display a victory or defeat message, update player stats, etc.

        // Set the battle result in the static BattleData class
        BattleData.PlayerWon = playerWon;

        // Wait for a short duration before transitioning back to the main scene
        yield return new WaitForSeconds(1f);

        // Transition back to the main scene
        TransitionToMainScene();
    }

    private void TransitionToMainScene()
    {
        // Load the main scene
        SceneManager.LoadScene("MainScene");
    }
}