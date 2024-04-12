using UnityEngine;
using TMPro;

public class UnitInfo : MonoBehaviour
{
    public GameObject infoPanel; // Assign your info panel in the inspector

    private TextMeshProUGUI nameText;
    private TextMeshProUGUI levelValueText;
    private TextMeshProUGUI healthValueText;

    private void OnMouseEnter()
    {
       

        // Enable the info panel
        infoPanel.SetActive(true);
        

        // Get the TextMeshProUGUI components
        TextMeshProUGUI[] texts = infoPanel.GetComponentsInChildren<TextMeshProUGUI>();
        nameText = texts[0]; // Assign these based on the order of your TextMeshProUGUI components in the prefab
        levelValueText = texts[3];
        healthValueText = texts[4];

       

        // Update the text
        UpdateInfo();
    }

    private void OnMouseExit()
    {
        

        // Disable the info panel
        infoPanel.SetActive(false);
        
    }

    private void UpdateInfo()
    {
        

        UnitPopup unitPopup = GetComponent<UnitPopup>();

        // Update the text components with the unit's data
        nameText.text = unitPopup.data.unitName;
        levelValueText.text = unitPopup.data.level.ToString();
        healthValueText.text = unitPopup.data.baseStats.health + "/" + unitPopup.data.baseStats.health; // Assuming current health is the same as max health

        
    }
}
