using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Player Health Bar")]
    public Image playerHealthBar;
    public TextMeshProUGUI playerHealthText;
    private int playerMaxHealth;
    private int playerCurrentHealth;

    [Header("Enemy Health Bar")]
    public Image enemyHealthBar;
    public TextMeshProUGUI enemyHealthText;
    private int enemyMaxHealth;
    private int enemyCurrentHealth;

    [Header("Player Name")]
    public TextMeshProUGUI playerNameText;

    [Header("Enemy Name")]
    public TextMeshProUGUI enemyNameText;

    private float lerpSpeed = 5f; // Speed of the health bar change

    private void Start()
    {
        // Initialize health values
        playerMaxHealth = BattleData.PlayerUnitHealth;
        playerCurrentHealth = playerMaxHealth;
        enemyMaxHealth = BattleData.EnemyUnitHealth;
        enemyCurrentHealth = enemyMaxHealth;

        UpdateHealthDisplay();
    }

    private void Update()
    {
        // Smoothly transition the health bar fill amount
        playerHealthBar.fillAmount = Mathf.Lerp(playerHealthBar.fillAmount, (float)playerCurrentHealth / playerMaxHealth, Time.deltaTime * lerpSpeed);
        enemyHealthBar.fillAmount = Mathf.Lerp(enemyHealthBar.fillAmount, (float)enemyCurrentHealth / enemyMaxHealth, Time.deltaTime * lerpSpeed);

        // Update text display continuously
        playerHealthText.text = $"{playerCurrentHealth}/{playerMaxHealth}";
        enemyHealthText.text = $"{enemyCurrentHealth}/{enemyMaxHealth}";
    }

    public void UpdatePlayerHealth(int health)
    {
        playerCurrentHealth = Mathf.Clamp(health, 0, playerMaxHealth);
        UpdateHealthDisplay();
    }

    public void UpdateEnemyHealth(int health)
    {
        enemyCurrentHealth = Mathf.Clamp(health, 0, enemyMaxHealth);
        UpdateHealthDisplay();
    }

    public void SetPlayerName(string name)
    {
        playerNameText.text = name;
    }

    public void SetEnemyName(string name)
    {
        enemyNameText.text = name;
    }

    private void UpdateHealthDisplay()
    {
        // Update health bar fill amount
        playerHealthBar.fillAmount = (float)playerCurrentHealth / playerMaxHealth;
        enemyHealthBar.fillAmount = (float)enemyCurrentHealth / enemyMaxHealth;

        // Update health text
        playerHealthText.text = $"{playerCurrentHealth}/{playerMaxHealth}";
        enemyHealthText.text = $"{enemyCurrentHealth}/{enemyMaxHealth}";
    }
}