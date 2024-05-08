using System.Collections;
using System.Collections.Generic;
using TbsFramework.Grid;
using TbsFramework.Players;
using TMPro;
using UnityEngine;

public class CurrentTurnUI : MonoBehaviour
{
    [SerializeField] private TMP_Text turnText;

    private CellGrid cellGrid;

    // Start is called before the first frame update
    void Start()
    {
        cellGrid = FindObjectOfType<CellGrid>();
        cellGrid.TurnEnded += CellGrid_TurnEnded;    
    }

    private IEnumerator UpdatePlayer()
    {
        yield return new WaitForSeconds(0.1f);
        turnText.text = cellGrid.CurrentPlayer is AIPlayer ? "Enemy Turn" : "Player Turn";
    }

    private void CellGrid_TurnEnded(object sender, bool isNetwork)
    {
        StartCoroutine(UpdatePlayer());
    }
}
