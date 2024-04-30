using UnityEngine;
using TMPro;

public class CombatPreviewUI : MonoBehaviour
{
    public TextMeshProUGUI attackerNameText;
    public TextMeshProUGUI attackerHPText;
    public TextMeshProUGUI attackerAttackText;
    public TextMeshProUGUI attackerDefenseText;

    public TextMeshProUGUI defenderNameText;
    public TextMeshProUGUI defenderHPText;
    public TextMeshProUGUI defenderAttackText;
    public TextMeshProUGUI defenderDefenseText;

    public void UpdateAttackerData(string name, int currentHP, int projectedHP, int attack, int defense)
    {
        attackerNameText.text = name;
        attackerHPText.text = $"HP: {currentHP} -> {projectedHP}";
        attackerAttackText.text = $"Attack: {attack}";
        attackerDefenseText.text = $"Defense: {defense}";
    }

    public void UpdateDefenderData(string name, int currentHP, int projectedHP, int attack, int defense)
    {
        defenderNameText.text = name;
        defenderHPText.text = $"HP: {currentHP} -> {projectedHP}";
        defenderAttackText.text = $"Attack: {attack}";
        defenderDefenseText.text = $"Defense: {defense}";
    }
}