using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerUnitPrefabEntry
    {
        public string unitName;
        public GameObject prefab;
    }

    // VARIABLES
    [Header("References")]
    [SerializeField] private RectTransform canvasParent;
    [SerializeField] private HealthBar playerHealthBar;
    [SerializeField] private HealthBar enemyHealthBar;

    [Header("Settings")]
    [SerializeField] private float startBattleDelay = 0.5f;
    [SerializeField] private float defenderDelay = 0.5f;
    [SerializeField] private float endBattleDelay = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool useDebug = true;
    [SerializeField] private UnitSO debugPlayerUnit;
    [SerializeField] private UnitSO debugEnemyUnit;

    private Animator playerAnimator;
    private Animator enemyAnimator;

    public static BattleManager Instance { get; private set; }

    // EXECUTION FUNCTIONS
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var playerInfo = BattleData.CurrentPlayerFighterData;
        var enemyInfo = BattleData.CurrentEnemyFighterData;

        if (useDebug)
        {
            playerAnimator = Instantiate(debugPlayerUnit.AttackPhasePrefab, canvasParent).GetComponent<Animator>();
            enemyAnimator = Instantiate(debugEnemyUnit.AttackPhasePrefab, canvasParent).GetComponent<Animator>();
        }
        else
        {
            playerAnimator = Instantiate(playerInfo.Data.AttackPhasePrefab, canvasParent).GetComponent<Animator>();
            enemyAnimator = Instantiate(enemyInfo.Data.AttackPhasePrefab, canvasParent).GetComponent<Animator>();
        }

        playerHealthBar.SetName(playerInfo.UnitName);
        playerHealthBar.SetMaxHealth(playerInfo.Data.TotalHealth);
        playerHealthBar.SetCurrentHealth(playerInfo.UnitHealth);

        enemyHealthBar.SetName(enemyInfo.UnitName);
        enemyHealthBar.SetMaxHealth(enemyInfo.Data.TotalHealth);
        enemyHealthBar.SetCurrentHealth(enemyInfo.UnitHealth);

        // Debug logs for initial health values
        Debug.Log($"BattleManager.Start: Player unit '{BattleData.CurrentPlayerFighterData.UnitName}' initial health: {BattleData.CurrentPlayerFighterData.UnitHealth}");
        Debug.Log($"BattleManager.Start: Enemy unit '{BattleData.CurrentEnemyFighterData.UnitName}' initial health: {BattleData.CurrentEnemyFighterData.UnitHealth}");

        // Start the battle sequence
        StartCoroutine(BattleSequence());
    }

    // METHODS
    public void ConnectAttack(UnitType unitType)
    {
        if (unitType == UnitType.Player)
        {
            int damage = CalculateDamage(BattleData.CurrentPlayerFighterData.UnitAttack, BattleData.CurrentEnemyFighterData.UnitDefence);
            BattleData.CurrentEnemyFighterData.LoseHealth(damage);
            enemyHealthBar.SetCurrentHealth(BattleData.CurrentEnemyFighterData.UnitHealth);

            enemyAnimator.Play("Defend");
        }
        else if (unitType == UnitType.Enemy)
        {
            int damage = CalculateDamage(BattleData.CurrentEnemyFighterData.UnitAttack, BattleData.CurrentPlayerFighterData.UnitDefence);
            BattleData.CurrentPlayerFighterData.LoseHealth(damage);
            playerHealthBar.SetCurrentHealth(BattleData.CurrentPlayerFighterData.UnitHealth);

            playerAnimator.Play("Defend");
        }
    }
    
    private IEnumerator BattleSequence()
    {
        yield return new WaitForSeconds(startBattleDelay);

        playerAnimator.Play("Attack");

        yield return new WaitForSeconds(0.1f);
        yield return new WaitWhile(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        yield return new WaitForSeconds(defenderDelay);

        if (BattleData.CurrentEnemyFighterData.UnitHealth > 0)
        {
            enemyAnimator.Play("Attack");

            yield return new WaitForSeconds(0.1f);
            yield return new WaitWhile(() => enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        }

        yield return new WaitForSeconds(endBattleDelay);

        yield return StartCoroutine(EndBattle(null));
    }

    private int CalculateDamage(int attackFactor, int defenseFactor)
    {
        int damage = Mathf.Max(0, attackFactor - defenseFactor);
        return damage;
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