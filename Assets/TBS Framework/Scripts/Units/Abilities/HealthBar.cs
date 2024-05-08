using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI nameText;
    private int maxHealth;
    private int currentHealth;

    [SerializeField] private float lerpSpeed = 5f; // Speed of the health bar change

    private void Update()
    {
        // Smoothly transition the health bar fill amount
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, (float)currentHealth / maxHealth, Time.deltaTime * lerpSpeed);

        // Update text display continuously
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    public void UpdatePlayerHealth(int health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthDisplay();
    }

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        UpdateHealthDisplay();
    }

    public void SetCurrentHealth(int currentHealth)
    {
        this.currentHealth = currentHealth;
        UpdateHealthDisplay();
    }

    private void UpdateHealthDisplay()
    {
        // Update health bar fill amount
        //fillImage.fillAmount = (float)currentHealth / maxHealth;

        // Update health text
        healthText.text = $"{currentHealth}/{maxHealth}";
    }
}