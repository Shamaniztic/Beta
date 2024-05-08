using System;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TbsFramework.Units;

namespace TbsFramework.Gui
{
    public class GUIController : MonoBehaviour
    {
        public CellGrid CellGrid;
        public Button EndTurnButton;
        public TMPro.TextMeshProUGUI UnitNameText;
        public TMPro.TextMeshProUGUI HealthText;
        public TMPro.TextMeshProUGUI AttackText;
        public TMPro.TextMeshProUGUI DefenceText;
        public GameObject InfoBox; // Reference to the info box GameObject

        void Awake()
        {
            CellGrid.LevelLoading += OnLevelLoading;
            CellGrid.LevelLoadingDone += OnLevelLoadingDone;
            CellGrid.GameEnded += OnGameEnded;
            CellGrid.TurnEnded += OnTurnEnded;
            CellGrid.GameStarted += OnGameStarted;
            CellGrid.UnitHighlighted += OnUnitHighlighted;
            CellGrid.UnitDehighlighted += OnUnitDehighlighted;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M) && CellGrid.CurrentPlayer is HumanPlayer && !(CellGrid.cellGridState is CellGridStateAITurn))
            {
                EndTurn(); //User ends his turn by pressing "m" on the keyboard.
            }
        }

        public void EndTurn()
        {
            CellGrid.EndTurn();
        }

        private void OnGameStarted(object sender, EventArgs e)
        {
            if (EndTurnButton != null)
            {
                EndTurnButton.interactable = CellGrid.CurrentPlayer is HumanPlayer;
                Debug.Log("Game Started: EndTurnButton is now " + (EndTurnButton.interactable ? "interactable" : "not interactable"));
            }
        }

        private void OnTurnEnded(object sender, bool isNetworkInvoked)
        {
            if (EndTurnButton != null)
            {
                EndTurnButton.interactable = CellGrid.CurrentPlayer is HumanPlayer;
                Debug.Log("Turn Ended: EndTurnButton is now " + (EndTurnButton.interactable ? "interactable" : "not interactable"));
            }
        }

        private void OnGameEnded(object sender, GameEndedArgs e)
        {
            Debug.Log($"Player{e.gameResult.WinningPlayers[0]} wins!");
            if (EndTurnButton != null)
            {
                EndTurnButton.interactable = false;
                Debug.Log("Game Ended: EndTurnButton is now not interactable");
            }
        }

        private void OnLevelLoading(object sender, EventArgs e)
        {
            Debug.Log("Level is loading");
        }

        private void OnLevelLoadingDone(object sender, EventArgs e)
        {
            Debug.Log("Level loading done");
        }

        private void OnUnitHighlighted(object sender, EventArgs e)
        {
            var unit = sender as Unit;
            if (unit != null)
            {
                UnitNameText.text = unit.UName;
                HealthText.text = string.Format("{0}/{1}", unit.HitPoints, unit.TotalHitPoints);
                AttackText.text = unit.AttackFactor.ToString();
                DefenceText.text = unit.DefenceFactor.ToString();

                InfoBox.SetActive(true); // Show the info box
            }
        }

        private void OnUnitDehighlighted(object sender, EventArgs e)
        {
            UnitNameText.text = "";
            HealthText.text = "";
            AttackText.text = "";
            DefenceText.text = "";

            InfoBox.SetActive(false); // Hide the info box
        }

    }
}
