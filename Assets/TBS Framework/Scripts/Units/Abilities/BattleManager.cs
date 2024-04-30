using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TbsFramework.Units;

public class BattleManager : MonoBehaviour
{
    public GameObject enemyUnitPrefab;
    [SerializeField] private List<PlayerUnitPrefabEntry> playerUnitPrefabEntries = new List<PlayerUnitPrefabEntry>();

    private Dictionary<string, GameObject> playerUnitPrefabs = new Dictionary<string, GameObject>();
    private Unit playerUnit;
    private Unit enemyUnit;
    private HealthBar healthBar;


    private void Awake()
    {
        // Convert the list of player unit prefab entries into a dictionary
        foreach (var entry in playerUnitPrefabEntries)
        {
            playerUnitPrefabs[entry.unitName] = entry.prefab;
        }
    }

    private void Start()
    {
        // Retrieve the unit data from BattleData
        string playerUnitName = BattleData.PlayerUnitName;
        int playerUnitPlayerNumber = BattleData.PlayerUnitPlayerNumber;
        int enemyUnitPlayerNumber = BattleData.EnemyUnitPlayerNumber;

        // Find the player unit prefab based on the unit name
        if (playerUnitPrefabs.TryGetValue(playerUnitName, out GameObject playerUnitPrefab))
        {
            // Get the Unit components from the prefabs
            playerUnit = playerUnitPrefab.GetComponent<Unit>();
            enemyUnit = enemyUnitPrefab.GetComponent<Unit>();

            if (playerUnit != null && enemyUnit != null)
            {
                // Set the player numbers and initialize the units
                playerUnit.PlayerNumber = playerUnitPlayerNumber;
                enemyUnit.PlayerNumber = enemyUnitPlayerNumber;
                playerUnit.Initialize();
                enemyUnit.Initialize();

                // Retrieve the health values from BattleData
                int playerUnitHealth = BattleData.PlayerUnitHealth;
                int enemyUnitHealth = BattleData.EnemyUnitHealth;

                // Set the health values for the player unit and enemy unit
                playerUnit.HitPoints = playerUnitHealth;
                enemyUnit.HitPoints = enemyUnitHealth;

                // Find the HealthBar script instance
                healthBar = FindObjectOfType<HealthBar>();

                // Set the unit names
                healthBar.SetPlayerName(BattleData.PlayerUnitName);
                healthBar.SetEnemyName(BattleData.EnemyUnitName);

                // Update the health bars
                healthBar.UpdatePlayerHealth(playerUnit.HitPoints);
                healthBar.UpdateEnemyHealth(enemyUnit.HitPoints);

                // Activate the unit prefabs
                playerUnitPrefab.SetActive(true);
                enemyUnitPrefab.SetActive(true);

                // Debug logs for initial health values
                Debug.Log($"BattleManager.Start: Player unit '{BattleData.PlayerUnitName}' initial health: {BattleData.PlayerUnitHealth}");
                Debug.Log($"BattleManager.Start: Enemy unit '{BattleData.EnemyUnitName}' initial health: {BattleData.EnemyUnitHealth}");

                // Assign the unit references to BattleData
                BattleData.PlayerUnit = playerUnit;
                BattleData.EnemyUnit = enemyUnit;

                // Start the battle sequence
                StartCoroutine(BattleSequence());
            }
            else
            {
                Debug.LogError("Player or enemy unit prefab is missing a Unit component.");
                TransitionToMainScene();
            }
        }
        else
        {
            Debug.LogError($"Player unit prefab not found for unit name: {playerUnitName}");
            TransitionToMainScene();
        }
    }

    [System.Serializable]
    public class PlayerUnitPrefabEntry
    {
        public string unitName;
        public GameObject prefab;
    }

    private IEnumerator BattleSequence()
    {
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
    }

    private bool IsUnitDefeated(Unit unit)
    {
        return unit.HitPoints <= 0;
    }

    private IEnumerator EndBattle(bool? playerWon)
    {
        // Debug logs for final health values
        Debug.Log($"BattleManager.EndBattle: Player unit '{BattleData.PlayerUnitName}' final health: {playerUnit.HitPoints}");
        Debug.Log($"BattleManager.EndBattle: Enemy unit '{BattleData.EnemyUnitName}' final health: {enemyUnit.HitPoints}");

        // Perform any necessary actions based on the battle outcome
        // Example: Display a victory or defeat message, update player stats, etc.

        // Set the battle result in the static BattleData class
        BattleData.PlayerWon = playerWon;

        // Update the BattleData with the final health values
        BattleData.PlayerUnit = playerUnit;
        BattleData.EnemyUnit = enemyUnit;
        BattleData.PlayerUnitHealth = playerUnit.HitPoints;
        BattleData.EnemyUnitHealth = enemyUnit.HitPoints;

        // Wait for a short duration before transitioning back to the main scene
        yield return new WaitForSeconds(2f);

        // Transition back to the main scene
        TransitionToMainScene();
    }

    private void TransitionToMainScene()
    {
        // Load the main scene
        SceneManager.LoadScene("MainScene");
    }
}