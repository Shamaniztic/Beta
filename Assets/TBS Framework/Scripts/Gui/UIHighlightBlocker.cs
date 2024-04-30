using UnityEngine;
using UnityEngine.EventSystems;
using TbsFramework.Cells;
using TbsFramework.Grid;

public class UIHighlightBlocker : MonoBehaviour
{
    private CellGrid cellGrid;
    private EventSystem eventSystem;

    private void Start()
    {
        cellGrid = FindObjectOfType<CellGrid>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (eventSystem != null && eventSystem.IsPointerOverGameObject())
        {
            BlockHighlighting();
        }
        else
        {
            UnblockHighlighting();
        }
    }

    private void BlockHighlighting()
    {
        if (cellGrid != null)
        {
            foreach (var cell in cellGrid.Cells)
            {
                cell.GetComponent<Square>().UnMark();
            }
        }
    }

    private void UnblockHighlighting()
    {
        // No need to do anything here, as the highlighting will resume automatically
        // when the mouse moves over the grid cells again.
    }
}