using UnityEngine;
using UnityEngine.UI;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TbsFramework.Grid.GridStates;

public class ActionMenu : MonoBehaviour
{
    public Button moveButton;
    public Button endTurnButton;
    public Button attackButton;

    private CellGrid cellGrid;
    private MoveAbility moveAbility;

    private void Start()
    {
        Debug.Log("ActionMenu.Start");
        attackButton.onClick.AddListener(OnAttackButtonClicked);
        moveButton.onClick.AddListener(OnMoveButtonClicked);
        endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);
    }

    public void SetMoveAbility(MoveAbility moveAbility)
    {
        Debug.Log($"ActionMenu.SetMoveAbility: moveAbility={moveAbility}");
        this.moveAbility = moveAbility;
    }

    public void SetCellGrid(CellGrid cellGrid)
    {
        Debug.Log($"ActionMenu.SetCellGrid: cellGrid={cellGrid}");
        this.cellGrid = cellGrid;
    }

    public void SetPosition(Vector3 unitPosition)
    {
        // Convert the unit's world position to screen position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(unitPosition);

        // Add an offset to position the menu to the right of the unit
        float offsetX = 100f; // Adjust this value as needed
        screenPosition.x += offsetX;

        // Set the position of the action menu's RectTransform
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = screenPosition;
    }

    public void ShowActionMenu()
    {
        Debug.Log("ActionMenu.ShowActionMenu");
        gameObject.SetActive(true);
    }

    private void OnMoveButtonClicked()
    {
        Debug.Log("ActionMenu.OnMoveButtonClicked");
        if (moveAbility != null)
        {
            Debug.Log("ActionMenu.OnMoveButtonClicked: Calling OnAbilitySelected on MoveAbility");
            moveAbility.OnAbilitySelected(cellGrid);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("ActionMenu.OnMoveButtonClicked: MoveAbility not found on unit");
        }
    }

    private void OnAttackButtonClicked()
    {
        Debug.Log("ActionMenu.OnAttackButtonClicked: Attempting to select AttackAbility");
        var attackAbility = moveAbility.UnitReference.GetComponent<AttackAbility>();

        if (attackAbility != null)
        {
            Debug.Log("ActionMenu.OnAttackButtonClicked: AttackAbility found, calling OnAbilitySelected");
            attackAbility.OnAbilitySelected(cellGrid);

            // Set the attack mode flag in the CellGridStateAbilitySelected state
            if (cellGrid.cellGridState is CellGridStateAbilitySelected cellGridStateAbilitySelected)
            {
                cellGridStateAbilitySelected.SetAttackModeActive(true);
            }

            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("ActionMenu.OnAttackButtonClicked: AttackAbility component not found on unit");
        }
    }


    private void OnEndTurnButtonClicked()
    {
        Debug.Log("ActionMenu.OnEndTurnButtonClicked");
        cellGrid.EndTurn();
        gameObject.SetActive(false);
    }
}